using log4net;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Repositories.Single;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Microsoft.DependencyInjection;

namespace ONS.PortalMQDI.Services.Services
{
    public class CargaService : ICargaService
    {
        public static string MensagemProcessamento { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(CargaService));

        #region CargaService
        private readonly ICalendarioSistemaRepository _calendarioSistemaRepository;
        private readonly ICalendarioCalculator _calendarioCalculator;
        private readonly CargaRepository _cargaRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRelatorioRepository _relatorioRepository;
        private readonly IValorParametroSistemaRepository _valorParametroSistemaRepository;
        private readonly EmailService _emailService;
        private readonly IAgenteRepository _agenteRepository;
        private IAwsService _awsService;
        public CargaService(
            ICalendarioSistemaRepository calendarioSistemaRepository,
            IFeriadoRepository feriadoRepository,
            ICalendarioCalculator calendarioCalculator,
            CargaRepository cargaRepository,
            IServiceProvider serviceProvider,
            IRelatorioRepository relatorioRepository,
            IValorParametroSistemaRepository valorParametroSistemaRepository,
            EmailService emailService, IAwsService awsService, IAgenteRepository agenteRepository)
        {
            _calendarioSistemaRepository = calendarioSistemaRepository;
            _calendarioCalculator = calendarioCalculator;
            _cargaRepository = cargaRepository;
            _serviceProvider = serviceProvider;
            _relatorioRepository = relatorioRepository;
            _valorParametroSistemaRepository = valorParametroSistemaRepository;
            _emailService = emailService;
            _awsService = awsService;
            _agenteRepository = agenteRepository;
        }
        #endregion

        public bool GerarCalendario(string anoMes)
        {
            var calendarios = _calendarioSistemaRepository.PegarTodosCalendario();

            if (calendarios.Any(c => c.DataReferenciaIndicador == anoMes))
            {
                throw new Exception($"A operação para o período {anoMes} já foi processada e não pode ser repetida.");
            }


            if (!_cargaRepository.VerificarDataExiste(anoMes.ConvertToAnomeReferencia()))
            {
                throw new Exception($"Não existem dados para o período {anoMes} a serem processados.");
            }

            DateTime dataReferenciaIndicador = anoMes.ConvertStringToDate();
            var calendarioParaSerAdicionado = _calendarioCalculator.CalculaCalendarioDoSistema();

            calendarioParaSerAdicionado.DataReferenciaIndicador = anoMes;
            if (_calendarioCalculator.VerificarSeCalendarioEstaValido(calendarioParaSerAdicionado))
            {
                if (_calendarioSistemaRepository.AdicionarRegistro(calendarioParaSerAdicionado))
                {
                    GerarRelatorio(new ProcessamentoCargaFilterViewModel
                    {
                        AnoMes = new List<string> { anoMes.ConvertToAnomeReferencia() },
                    });
                    return true;
                }
            }

            return false;
        }

        public bool GerarRelatorio(ProcessamentoCargaFilterViewModel viewModel)
        {
            var scope = _serviceProvider.CreateScope();
            Task.Run(async () =>
            {
                try
                {
                    viewModel.AnoMes = viewModel.AnoMes.Select(c => c.ConvertToAnomeReferencia()).ToList();
                    await ProcessarAnoMes(viewModel, scope.ServiceProvider);
                }
                catch (Exception ex)
                {
                    MensagemProcessamento = $"ERROR#{ex.Message}";
                    ex.LogErrorWithNumber(log);
                    throw;
                }
                finally
                {
                    scope.Dispose(); // Descartar o escopo manualmente
                }
            });

            return true;
        }

        private async Task ProcessarAnoMes(ProcessamentoCargaFilterViewModel viewModel, IServiceProvider serviceProvider)
        {
            var agenteLista = new List<Agente>();
            var tpRelatorioRepository = serviceProvider.GetRequiredService<ITpRelatorioRepository>();
            var agenteRepository = serviceProvider.GetRequiredService<IAgenteRepository>();
            var instalacaoService = serviceProvider.GetRequiredService<IInstalacaoService>();
            var indicadorRepository = serviceProvider.GetRequiredService<ITpIndicadorRepository>();
            var cargaRepository = serviceProvider.GetRequiredService<CargaRepository>();
            var relatorioService = serviceProvider.GetRequiredService<IRelatorioService>();
            var grandezaRepository = serviceProvider.GetRequiredService<IGrandezaRepository>();
            var relatorioRepository = serviceProvider.GetRequiredService<IRelatorioRepository>();

            var tipoRelatorio = await tpRelatorioRepository.GetAllAsync(CancellationToken.None);
            var agenteEntry = await agenteRepository.GetAsync(c => viewModel.AnoMes.Contains(c.AnoMesReferencia), CancellationToken.None);
            var tpIndicadores = await indicadorRepository.GetAllAsync(CancellationToken.None);
            var relatorio = await relatorioRepository.GetAsync(c => viewModel.AnoMes.Select(c => c.ConvertToAnomeReferencia()).Contains(c.AnomesReferencia), CancellationToken.None);

            if (!tipoRelatorio.Any() || !agenteEntry.Any())
            {
                MensagemProcessamento = $"ERROR#Ocorreu um erro ao processar relatorio - {string.Join(", ", viewModel.AnoMes)}";
                throw new Exception($"Ocorreu um erro ao processar relatorio - {string.Join(", ", viewModel.AnoMes)}");
            }

            if (viewModel.AgeMrid != null && viewModel.AgeMrid.Count() > 0)
            {
                agenteLista = agenteEntry.Where(c => viewModel.AgeMrid.Contains(c.AgeMrid)).ToList();
            }
            else
            {
                agenteLista = agenteEntry.ToList();
            }

            CargaConfig.Iniciar();
            CargaConfig.TotalAgente = agenteLista.Count();
            CargaConfig.Poncentagem = 0.1;
            foreach (var data in viewModel.AnoMes)
            {
                var groupRelatorio = relatorio.Where(c => c.AnomesReferencia == data.ConvertToAnomeReferencia()).ToList();
                await ProcessarRelatorio(
                    data.ConvertToAnomeReferencia(),
                    agenteLista,
                    tipoRelatorio.ToList(),
                    tpIndicadores.ToList(),
                    cargaRepository,
                    relatorioService,
                    instalacaoService,
                    grandezaRepository,
                    groupRelatorio,
                    viewModel.ProcRelatorioTipo
                );
            }
        }

        private async Task ProcessarRelatorio(string mesAno, List<Agente> agenteSubset, List<TpRelatorio> tipoRelatorio, List<TpIndicador> indicadores, CargaRepository cargaRepository, IRelatorioService relatorioService, IInstalacaoService instalacaoService, IGrandezaRepository grandezaRepository, List<Relatorio> groupRelatorio, string ProcRelatorioTipo)
        {
            var agenteFilter = agenteSubset.DistinctBy(c => c.AgeMrid).OrderBy(c => c.NomeCurto).ToList();

            MensagemProcessamento = $"LOG#Agente: {agenteFilter.Count()} - Relatorio: ${tipoRelatorio.Count()} Início do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
            log.Info($"[Agente: {agenteFilter.Count()} Relatorio: ${tipoRelatorio.Count()}]Início do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");

            if (ProcRelatorioTipo.Equals("all"))
            {
                foreach (var agente in agenteFilter)
                {
                    var queryInstalacao = await grandezaRepository.InstalacaoConsultarMedidaAsync(agente.AgeMrid, mesAno, CancellationToken.None);
                    var automacao = new AutomacaoIndicador
                    {
                        EntryMedidas = instalacaoService.RetornaMedidasSupervisionadasRecursos(queryInstalacao),
                        EntryAgentes = cargaRepository.BuscarAgente(mesAno, agente.AgeMrid).Cast<object>().ToList(),
                        EntryResultadoIndicador = cargaRepository.BuscarSSCL(mesAno, agente.AgeMrid).Cast<object>().ToList(),
                        EntryInstalacao = cargaRepository.BuscarInstalacaoRecurso(mesAno, agente.AgeMrid).Cast<object>().ToList(),
                        EntryIndicadores = indicadores.Cast<object>().ToList(),
                        AnoMes = mesAno,
                        NomeAgente = agente.NomeCurto,
                        IdAgente = agente.AgeMrid
                    };

                    if (automacao.EntryAgentes.Any() || automacao.EntryResultadoIndicador.Any() || automacao.EntryInstalacao.Any())
                    {
                        foreach (var relatorio in tipoRelatorio)
                        {
                            foreach (var indicador in indicadores)
                            {
                                var existe = groupRelatorio.Any(c => c.AgenteId == agente.AgeMrid && c.IdTpRelatorio == relatorio.IdTpRelatorio && c.IdTpIndicador == indicador.Id);

                                if (relatorio.Codigo == nameof(TipoRelatorioEnum.RAvDQ) && !existe)
                                {
                                    // Demanda solicitou desativação - 162907
                                    //ConfigurarAutomacao(automacao, relatorio.Codigo, agente.IdOns, mesAno);
                                    //relatorioService.GerarRMCViolacoesQualidade(automacao);
                                }
                                else if (relatorio.Codigo == nameof(TipoRelatorioEnum.RAcDQ) && !existe)
                                {
                                    // Demanda solicitou desativação - 162907
                                    //ConfigurarAutomacao(automacao, relatorio.Codigo, agente.IdOns, mesAno);
                                    //relatorioService.GerarRMCAcompanhamentoQualidade(automacao);
                                }
                                else if (relatorio.Codigo == nameof(TipoRelatorioEnum.RAiDQ) 
                                    && !groupRelatorio.Any(c => c.AgenteId == agente.AgeMrid && c.IdTpRelatorio == relatorio.IdTpRelatorio))
                                {
                                    ConfigurarAutomacao(automacao, relatorio.Codigo, agente.IdOns, mesAno);
                                    relatorioService.GerarRMIndicadoresQualidade(automacao);
                                }
                            }
                        }
                    }

                    CargaConfig.AgenteProcessado++;
                    CargaConfig.Poncentagem = (double)CargaConfig.AgenteProcessado / CargaConfig.TotalAgente * 100;
                }

            }
            
            if (tipoRelatorio.Any(c => c.Codigo == nameof(TipoRelatorioEnum.RAmD)) && !groupRelatorio.Any(c => c.AgenteId == "ONS"))
            {
                var datas = mesAno.GeneratePastMonths(3);
                var grandezaEntry = grandezaRepository.BuscarInstalacaoRangerData(datas);
                relatorioService.GerarRMManutençãoDisponibildade(grandezaEntry.ToList(), mesAno);
            }


            MensagemProcessamento = $"OK# Agente: {agenteFilter.Count()} - Relatorio: ${tipoRelatorio.Count()} Fim do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
            log.Info($"[Agente: {agenteFilter.Count()} Relatorio: ${tipoRelatorio.Count()}]Fim do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
        }

        private void ConfigurarAutomacao(AutomacaoIndicador automacao, string codigoRelatorio, string idOns, string mesAno)
        {
            string fixHot = string.Empty;

            Enum.TryParse(codigoRelatorio, out TipoRelatorioEnum tipoRelatorioEnum);
            automacao.TipoRelatorio = (int)tipoRelatorioEnum;

            if (tipoRelatorioEnum == TipoRelatorioEnum.RAiDQ)
            {
                fixHot = "_";
            }
            else
            {
                fixHot = "_!indicador!_";
            }
            automacao.Arquivo = new ArquivoViewModel
            {
                NomeArquivo = $"{codigoRelatorio}_{idOns.Trim()}{fixHot}{mesAno.Replace("-", "_")}",
                PastaArquivo = idOns.Trim()
            };
        }

        public async Task<string> DeletarRelatorioAsync(ProcessamentoCargaFilterViewModel viewModel, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.True<Relatorio>();

            if (viewModel.AnoMes != null && viewModel.AnoMes.Count() > 0)
            {
                predicate = predicate.And(p => viewModel.AnoMes.Select(c => c.ConvertToAnomeReferencia()).Contains(p.AnomesReferencia));
            }

            if (viewModel.AgeMrid != null && viewModel.AgeMrid.Count() > 0)
            {
                predicate = predicate.And(p => viewModel.AgeMrid.Contains(p.AgenteId));
            }

            var scope = _serviceProvider.CreateScope();
            var relatorios = await _relatorioRepository.BuscarAgenteAsync(predicate, cancellationToken);

            var tempRelatorios = relatorios.DistinctBy(c => c.IdRelatorio);

            var t = Task.Run(async () =>
            {
                try
                {
                    var tpRelatorioRepository = scope.ServiceProvider.GetRequiredService<IRelatorioRepository>();
                    await RemoverS3Async(tempRelatorios);
                    await tpRelatorioRepository.DeletarPorFiltroAsync(predicate, cancellationToken);
                    log.Info("Relatórios removidos");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Error(ex.StackTrace);

                    throw;
                }
            });
     
            return "Esta sendo processado, avisaremos quando concluirmos.";
        }

        private async Task RemoverS3Async(IEnumerable<Relatorio> relatorios)
        {
            foreach (var item in relatorios)
            {
                string pasta = String.Empty;
                string arquivo = String.Empty;

                if (item.TpRelatorio.Codigo == nameof(TipoRelatorioEnum.RAiDQ))
                {
                    pasta = item.Agente.IdOns;
                    arquivo = $"{item.TpRelatorio.Codigo.Trim()}_{item.Agente.IdOns.Trim()}_{item.AnomesReferencia.Replace("-", "_")}.xlsx";
                }
                else if (item.TpRelatorio.Codigo == nameof(TipoRelatorioEnum.RAmD))
                {
                    pasta = nameof(TipoRelatorioEnum.RAmD);
                    arquivo = $"{nameof(TipoRelatorioEnum.RAmD).Trim()}_{item.AnomesReferencia.Replace("-", "_")}.xlsx";
                }
                else
                {
                    pasta = item.Agente.IdOns;
                    arquivo = $"{item.TpRelatorio.Codigo.Trim()}_{item.Agente.IdOns.Trim()}_{item.TpIndicador.CodIndicador.Trim()}_{item.AnomesReferencia.Replace("-", "_")}.pdf";
                }

                log.Info($"[Pasta: {pasta} Arquivo: {arquivo}]");

                var result = await _awsService.RemoverFileAsync($"{pasta.Trim().Replace(" ", "")}/{arquivo.Trim().Replace(" ", "")}", CancellationToken.None);

            }
        }


        public StatusCarga StatusRelatorio()
        {

            if (!string.IsNullOrEmpty(MensagemProcessamento))
            {
                string[] log = MensagemProcessamento.Split('#');
                var model = new StatusCarga();

                if (CargaEnum.ERROR.ToString().Equals(log[0]))
                {
                    model.Status = CargaEnum.ERROR.ToString();
                    model.Mensagem = log[1];
                }
                else if (CargaEnum.OK.ToString().Equals(log[0]))
                {
                    model.PoncetagemAgente = null;
                    model.Status = CargaEnum.OK.ToString();
                    model.Mensagem = log[1];
                }
                else if (CargaEnum.LOG.ToString().Equals(log[0]))
                {
                    model.Status = CargaEnum.LOG.ToString();
                    model.Mensagem = log[1];
                }

                return model;
            }

            return null;
        }

        public async Task<RelatorioDisponivelViewModel> RelatorioDisponivelAsync(CancellationToken cancellationToken)
        {
            var tempRelatorioDisponivel = new RelatorioDisponivelViewModel();
            var relatorioEntries = await _relatorioRepository.BuscarTodosRelatorioAsync(cancellationToken);
            var agentesEntry = await _agenteRepository.TodosAgentePorGrandezaAsync(null, cancellationToken);

            tempRelatorioDisponivel.Data = relatorioEntries
                .Select(c => c.AnomesReferencia.ConvertAnomeReferenciaToDate())
                .Distinct()
                .Select(date => new SelectItemViewModel<string>
                {
                    Id = date.ConvertToAnomeReferencia(),
                    Value = date.ConvertToAnomeReferencia(),
                    Label = date.ToString()
                })
                .OrderByDescending(c => c.Id).ToList();

            tempRelatorioDisponivel.Agente = agentesEntry.DistinctBy(c=> c.IdOns)
                .Select(agente => new SelectItemViewModel<string>
                {
                    Id = agente.AgeMrid,
                    Value = agente.AgeMrid,
                    Label = agente.NomeCurto
                })
               .OrderBy(c => c.Label).ToList();

            return tempRelatorioDisponivel;
        }

        public bool GerarRelatorio()
        {
            var filtro = new ProcessamentoCargaFilterViewModel();
            filtro.Centro = new List<string>();

            var listaCentro = Enum.GetValues(typeof(CentroOperacaoEnum)).OfType<CentroOperacaoEnum>().ToList();
            var anoMes = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");

            foreach (var centro in listaCentro)
            {
                if (_relatorioRepository.ConsultarCentroProcessamento(centro.ToString(), anoMes))
                {
                    filtro.Centro.Add(centro.ToString());
                }
            }

            if (filtro.Centro.Count > 0)
            {
                var scope = _serviceProvider.CreateScope();
                Task.Run(async () =>
                {
                    try
                    {
                        await ProcessarAnoMes(filtro, scope.ServiceProvider);
                        await DisparoEmailFim(true, String.Join(", ", filtro.Centro), anoMes);
                    }
                    catch (Exception ex)
                    {
                        ex.LogErrorWithNumber(log);
                        await DisparoEmailFim(false, String.Join(", ", filtro.Centro), anoMes);
                        throw;
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                });
            }

            return true;
        }

        private async Task DisparoEmailFim(bool sucesso, string centros, string anoMes)
        {
            var listaEmail = await _valorParametroSistemaRepository
                .GetAsync(c => c.ValParametro.Contains("@") && c.DataFimVigencia == null, CancellationToken.None);

            string tituloEmail;
            string corpoEmail;

            if (sucesso)
            {
                tituloEmail = $"Automação de Calendário: Geração bem-sucedida para {anoMes} e centros {centros}";
                corpoEmail = $"Prezados(as),\n\nInformamos que a automação de geração de calendário foi concluída com sucesso para o mês e ano {anoMes} nos seguintes centros: {centros}.";
            }
            else
            {
                tituloEmail = $"Automação de Calendário: Erro na geração para {anoMes} e centros {centros}";
                corpoEmail = $"Prezados(as),\n\nLamentamos informar que ocorreu um erro durante a automação de geração de calendário para o mês e ano {anoMes} nos seguintes centros: {centros}.\nPor favor, verifique o sistema para mais detalhes.";
            }

            await _emailService.SendEmailAsync(listaEmail.Select(c => c.ValParametro).ToList(), tituloEmail, corpoEmail);
        }
    }
}
