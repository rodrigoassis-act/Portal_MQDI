using log4net;
using Microsoft.Extensions.DependencyInjection;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Repositories.Single;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Constants;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class CalendarioService : ICalendarioService
    {
        public static string MensagemProcessamento { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(CalendarioService));

        private readonly ICalendarioSistemaRepository _calendarioSistemaRepository;
        private readonly IValorParametroSistemaRepository _valorParametroSistemaRepository;
        private readonly IFeriadoRepository _feriadoRepository;
        private readonly IRelatorioRepository _relatorioRepository;
        private readonly ICalendarioCalculator _calendarioCalculator;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogEventoService _logEventoService;
        private readonly ICargaService _cargaService;
        public CalendarioService(ICalendarioSistemaRepository calendarioSistemaRepository,
            IValorParametroSistemaRepository valorParametroSistemaRepository,
            IFeriadoRepository feriadoRepository, IRelatorioRepository relatorioRepository,
            ICalendarioCalculator calendarioCalculator, IServiceProvider serviceProvider, ILogEventoService logEventoService, ICargaService cargaService)
        {
            _calendarioSistemaRepository = calendarioSistemaRepository;
            _valorParametroSistemaRepository = valorParametroSistemaRepository;
            _feriadoRepository = feriadoRepository;
            _relatorioRepository = relatorioRepository;
            _calendarioCalculator = calendarioCalculator;
            _serviceProvider = serviceProvider;
            _logEventoService = logEventoService;
            _cargaService = cargaService;
        }
        public async Task<List<string>> DatasDisponiveisParaSelecaoAsync(CancellationToken cancellationToke)
        {
            var entryCalendario = await _calendarioSistemaRepository.GetAllAsync(cancellationToke);

            return entryCalendario.Select(c => c.DataReferenciaIndicador.Replace("-", "/")).OrderByDescending(c => c).ToList();
        }

        public async Task<CalendarioViewModel> RetornaDatasDoSistemaAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            var entryCalendario = await _calendarioSistemaRepository.GetAsync(c => c.DataReferenciaIndicador == filtro.MesAnoFormatada(), cancellationToke);


            if (entryCalendario.Count() == 0)
            {
                return new CalendarioViewModel();
            }

            var calendario = entryCalendario.FirstOrDefault();

            var parametro = await _valorParametroSistemaRepository.BuscarParametroTerminoPeriodoAnaliseContestacaoAsync(cancellationToke);

            return new CalendarioViewModel
            {
                DataLiberacaoIndicadoresAgente = calendario.DataLiberacaoContestacoesAgente.ToString("dd/MM/yyyy"),
                DataTerminoContestacaoAgente = calendario.DataTerminoPeriodoContestacoesAgente.ToString("dd/MM/yyyy"),
                TerminoPeriodoConsolidacaoONS = calendario.DataTerminoPeriodoAnaliseContestacoes.ToString("dd/MM/yyyy"),
                DataInicioAlertaFimPrazoONS = calendario.DataTerminoPeriodoAnaliseContestacoes.AddDays(-Int32.Parse(parametro.ValParametro)).ToString("dd/MM/yyyy")
            };
        }

        public async Task<bool> RecalculaCalendarioSistemaPorMudancaDeParametroAsync(CancellationToken cancellationToken)
        {
            CalendarioSistema calendario = _calendarioSistemaRepository.GetAllAsync(cancellationToken).Result.LastOrDefault();
            if (calendario != null)
            {
                CalendarioSistema novoCalendario = AtualizaCalendarioPorMudancaDeParametro(calendario, cancellationToken);
                DateTime dataDeHoje = DateTime.Now;

                calendario.DataLiberacaoContestacoesAgente = novoCalendario.DataLiberacaoContestacoesAgente;
                calendario.DataTerminoPeriodoContestacoesAgente = novoCalendario.DataTerminoPeriodoContestacoesAgente;
                calendario.DataTerminoPeriodoAnaliseContestacoes = novoCalendario.DataTerminoPeriodoAnaliseContestacoes;
                await _calendarioSistemaRepository.UpdateAsync(calendario.IdCalendarioSistema, calendario, cancellationToken);
            }
            return true;
        }

        public CalendarioSistema AtualizaCalendarioPorMudancaDeParametro(CalendarioSistema calendarioVelho, CancellationToken cancellationToken)
        {
            CalendarioSistema calendario = new CalendarioSistema();

            var nomeParametroDiaUtilOuCorrido = ApplicationConstants.ParaMetroDiaUtilOuCorrido;
            var parametroDiaUtilOuCorrido = _valorParametroSistemaRepository.RetornaValorAtualDoParametroPorNome(nomeParametroDiaUtilOuCorrido).ValParametro == ApplicationConstants.ValorParametroParaDiaUtil;
            var parametros = _valorParametroSistemaRepository.BuscaValorParametroSistemaAsync(cancellationToken)
                .Result
                .Where(p => p.ParametroSistema.NomeParametro != nomeParametroDiaUtilOuCorrido)
                .ToList();

            var feriadosNoMes = _feriadoRepository.ListaFeriadosNoMes(calendarioVelho.DataReferenciaIndicador).ToList();
            calendario.DataLiberacaoContestacoesAgente = DefinirDataLiberacao(parametroDiaUtilOuCorrido, parametros, calendarioVelho.DataLiberacaoContestacoesAgente, feriadosNoMes);
            calendario.DataTerminoPeriodoContestacoesAgente = DefinirDataTerminoContestacao(parametroDiaUtilOuCorrido, parametros, calendarioVelho.DataTerminoPeriodoContestacoesAgente, feriadosNoMes, calendario.DataLiberacaoContestacoesAgente);
            calendario.DataTerminoPeriodoAnaliseContestacoes = DefinirDataTerminoPeriodoAnaliseContestacoes(parametroDiaUtilOuCorrido, parametros, calendarioVelho.DataTerminoPeriodoAnaliseContestacoes, feriadosNoMes, calendario.DataTerminoPeriodoContestacoesAgente);

            return calendario;
        }

        private DateTime DefinirDataLiberacao(bool parametroDiaUtilOuCorrido, List<ValorParametroSistema> parametros, DateTime dataLiberacaoContestacoesAgente, List<Feriado> feriadosNoMes)
        {
            DateTime dataLiberacao = new DateTime();
            if (parametroDiaUtilOuCorrido)
            {
                var parametroLiberacao =
                parametros.FirstOrDefault(p => p.ParametroSistema.NomeParametro == ApplicationConstants.ParametroLiberacaoAgente);
                dataLiberacao = BuscaDatasDosParametros(parametroLiberacao, feriadosNoMes);
            }
            else
            {
                var parametroLiberacao =
                    parametros.FirstOrDefault(p => p.ParametroSistema.NomeParametro == ApplicationConstants.ParametroLiberacaoAgente);
                dataLiberacao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dataLiberacao = dataLiberacao.AddDays(Int32.Parse(parametroLiberacao.ValParametro) - 1);
            }
            var dataHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (dataLiberacaoContestacoesAgente < dataHoje || dataLiberacao < dataHoje)
            {
                dataLiberacao = dataLiberacaoContestacoesAgente;
            }
            return dataLiberacao;
        }

        private DateTime BuscaDatasDosParametros(ValorParametroSistema p, List<Feriado> feriadosNoMes, DateTime? dataDoUltimoParametro = null)
        {
            var valorDoParametro = int.Parse(p.ValParametro);
            DateTime dataDoParametro, startDate;

            if (dataDoUltimoParametro != null)
            {
                dataDoParametro = dataDoUltimoParametro.Value.AddDays(1);
                startDate = dataDoUltimoParametro.Value.AddDays(1);
            }
            else
            {
                dataDoParametro = DateTime.Today.AddDays(1);
                startDate = DateTime.Today.AddDays(1);
            }
            var endDate = startDate.AddMonths(2).AddDays(-1);

            //VERIFICA FERIADOS E FINS DE SEMANA PARA MONTAR A DATA
            while (valorDoParametro > 0)
            {
                if (startDate > endDate)
                {
                    throw new Exception("Parâmetros cadastrados estão ultrapassando o limite de 2 meses");
                }
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (VerificaSeDataEDiaUtil(date, feriadosNoMes))
                    {
                        valorDoParametro--;
                    }
                    if (valorDoParametro == 0)
                    {
                        break;
                    }
                    else
                    {
                        dataDoParametro = dataDoParametro.AddDays(1);
                    }
                }
            };

            return dataDoParametro;

        }

        private bool VerificaSeDataEDiaUtil(DateTime date, List<Feriado> listaDeFeriadosDoMes)
        {
            var cond1 = ((date.DayOfWeek >= DayOfWeek.Monday) && (date.DayOfWeek <= DayOfWeek.Friday));
            var cond2 = true;

            listaDeFeriadosDoMes.ForEach(f =>
            {
                if (f.DataFeriado.Value.Day == date.Date.Day && f.DataFeriado.Value.Month == date.Date.Month)
                {
                    cond2 = false;
                }
            });

            return cond1 && cond2;
        }

        private DateTime DefinirDataTerminoContestacao(bool parametroDiaUtilOuCorrido, List<ValorParametroSistema> parametros, DateTime dataTerminoPeriodoContestacoesAgente, List<Feriado> feriadosNoMes, DateTime dataLiberacao)
        {
            DateTime dataTerminoContestacao = new DateTime();
            if (parametroDiaUtilOuCorrido)
            {
                var parametroTerminoPeridoContestacao = parametros.Where(p => p.ParametroSistema.NomeParametro ==
                        ApplicationConstants.ParametroTerminoPeriodoContestacao).FirstOrDefault();
                dataTerminoContestacao = BuscaDatasDosParametros(parametroTerminoPeridoContestacao, feriadosNoMes, dataLiberacao);
            }
            else
            {
                var parametroTerminoPeridoContestacao =
                    parametros.Where(p => p.ParametroSistema.NomeParametro ==
                    ApplicationConstants.ParametroTerminoPeriodoContestacao).FirstOrDefault();
                dataTerminoContestacao = dataLiberacao.AddDays(Int32.Parse(parametroTerminoPeridoContestacao.ValParametro));
            }
            var dataHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (dataTerminoPeriodoContestacoesAgente < dataHoje || dataTerminoContestacao < dataHoje)
            {
                dataTerminoContestacao = dataTerminoPeriodoContestacoesAgente;
            }
            return dataTerminoContestacao;
        }

        private DateTime DefinirDataTerminoPeriodoAnaliseContestacoes(bool parametroDiaUtilOuCorrido, List<ValorParametroSistema> parametros, DateTime dataTerminoPeriodoAnaliseContestacoes, List<Feriado> feriadosNoMes, DateTime dataTerminoContestacao)
        {
            DateTime dataTerminoPeriodoAnaliseONS = new DateTime();
            if (parametroDiaUtilOuCorrido)
            {
                var parametroTerminoPeriodoAnaliseONS = parametros.Where(p => p.ParametroSistema.NomeParametro ==
                        ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao).FirstOrDefault();
                dataTerminoPeriodoAnaliseONS = BuscaDatasDosParametros(parametroTerminoPeriodoAnaliseONS, feriadosNoMes, dataTerminoContestacao);
            }
            else
            {
                var parametroTerminoPeriodoAnaliseONS = parametros.Where(p => p.ParametroSistema.NomeParametro ==
                        ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao).FirstOrDefault();
                dataTerminoPeriodoAnaliseONS = dataTerminoContestacao.AddDays(Int32.Parse(parametroTerminoPeriodoAnaliseONS.ValParametro));
            }
            var dataHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (dataTerminoPeriodoAnaliseContestacoes < dataHoje || dataTerminoPeriodoAnaliseONS < dataHoje)
            {
                var parametroTerminoPeriodoAnaliseONS = parametros.Where(p => p.ParametroSistema.NomeParametro ==
                        ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao).FirstOrDefault();
                dataTerminoPeriodoAnaliseONS = dataTerminoContestacao.AddDays(Int32.Parse(parametroTerminoPeriodoAnaliseONS.ValParametro));
            }
            return dataTerminoPeriodoAnaliseONS;
        }

        public async Task<bool> GeraCalendarioDoSistemaAsync(string mesAno, CancellationToken cancellationToken)
        {
            var calendariosGerados = _calendarioSistemaRepository.GetAllAsync(cancellationToken).Result.OrderByDescending(c => c.DataReferenciaIndicador);
            DateTime dataReferenciaIndicador = DateTime.ParseExact(ApplicationConstants.DataInicialIndicadores + mesAno, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);
            var calendarioParaSerAdicionado = CalculaCalendarioDoSistema(cancellationToken);
            calendarioParaSerAdicionado.DataReferenciaIndicador = dataReferenciaIndicador.ToString("yyyy-MM");
            if (VerificarSeCalendarioEstaValido(calendarioParaSerAdicionado))
            {
                await _calendarioSistemaRepository.InsertAsync(calendarioParaSerAdicionado, cancellationToken);
                await _calendarioSistemaRepository.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        private CalendarioSistema CalculaCalendarioDoSistema(CancellationToken cancellationToken)
        {
            CalendarioSistema calendario = new CalendarioSistema();

            var nomeParametroDiaUtilOuCorrido =
                ApplicationConstants.ParaMetroDiaUtilOuCorrido;
            var valorParaDiaUtil = ApplicationConstants.ValorParametroParaDiaUtil;

            var valorParametroDiaUtilOuCorrido = _valorParametroSistemaRepository.RetornaValorAtualDoParametroPorNome(nomeParametroDiaUtilOuCorrido).ValParametro;
            var parametros = _valorParametroSistemaRepository.BuscaValorParametroSistemaAsync(cancellationToken).Result.Where(p => p.ParametroSistema.NomeParametro != nomeParametroDiaUtilOuCorrido).ToList();

            DateTime dataLiberacao, dataTerminoContestacao, dataTerminoPeriodoAnaliseONS;

            if (valorParametroDiaUtilOuCorrido == valorParaDiaUtil)
            {
                DateTime dataParaCalcular = DateTime.Now;
                var feriadosNoMes = _feriadoRepository.ListaFeriadosNoMes(dataParaCalcular.ToString("yyyy-MM")).ToList();
                var parametroLiberacao =
                    parametros.FirstOrDefault(p => p.ParametroSistema.NomeParametro == ApplicationConstants.ParametroLiberacaoAgente);
                dataLiberacao = BuscaDatasDosParametros(parametroLiberacao, feriadosNoMes);

                var parametroTerminoPeridoContestacao =
                    parametros.FirstOrDefault(p => p.ParametroSistema.NomeParametro ==
                    ApplicationConstants.ParametroTerminoPeriodoContestacao);
                dataTerminoContestacao = BuscaDatasDosParametros(parametroTerminoPeridoContestacao, feriadosNoMes, dataLiberacao);

                var parametroTerminoPeriodoAnaliseONS =
                    parametros.FirstOrDefault(p => p.ParametroSistema.NomeParametro ==
                    ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao);
                dataTerminoPeriodoAnaliseONS = BuscaDatasDosParametros(parametroTerminoPeriodoAnaliseONS, feriadosNoMes, dataTerminoContestacao);
            }
            else
            {
                var parametroLiberacao =
                    parametros.Where(p => p.ParametroSistema.NomeParametro == ApplicationConstants.ParametroLiberacaoAgente).FirstOrDefault();
                dataLiberacao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dataLiberacao = dataLiberacao.AddDays(Int32.Parse(parametroLiberacao.ValParametro) - 1);

                var parametroTerminoPeridoContestacao =
                    parametros.Where(p => p.ParametroSistema.NomeParametro ==
                    ApplicationConstants.ParametroTerminoPeriodoContestacao).FirstOrDefault();
                dataTerminoContestacao = dataLiberacao.AddDays(Int32.Parse(parametroTerminoPeridoContestacao.ValParametro));

                var parametroTerminoPeriodoAnaliseONS =
                    parametros.Where(p => p.ParametroSistema.NomeParametro ==
                    ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao).FirstOrDefault();
                dataTerminoPeriodoAnaliseONS = dataTerminoContestacao.AddDays(Int32.Parse(parametroTerminoPeriodoAnaliseONS.ValParametro));
            }

            calendario.DataLiberacaoContestacoesAgente = dataLiberacao;
            calendario.DataTerminoPeriodoContestacoesAgente = dataTerminoContestacao;
            calendario.DataTerminoPeriodoAnaliseContestacoes = dataTerminoPeriodoAnaliseONS;

            return calendario;
        }

        private bool VerificarSeCalendarioEstaValido(CalendarioSistema calendario)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1) >= calendario.DataTerminoPeriodoAnaliseContestacoes;
        }

        public bool GeraCalendarioDoSistema(string anoMes, CancellationToken cancellationToken)
        {
            var mesAno = anoMes.Split('-')[1] + "/" + anoMes.Split('-')[0];
            var calendariosGerados = _calendarioSistemaRepository.GetAllAsync(cancellationToken).Result.OrderByDescending(c => c.DataReferenciaIndicador);
            DateTime dataReferenciaIndicador = DateTime.ParseExact(ApplicationConstants.DataInicialIndicadores + mesAno, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);
            var calendarioParaSerAdicionado = CalculaCalendarioDoSistema(cancellationToken);
            calendarioParaSerAdicionado.DataReferenciaIndicador = dataReferenciaIndicador.ToString("yyyy-MM");
            if (VerificarSeCalendarioEstaValido(calendarioParaSerAdicionado))
            {
                _calendarioSistemaRepository.InsertAsync(calendarioParaSerAdicionado, cancellationToken);
                _calendarioSistemaRepository.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<CalendarioFullViewModel> BuscaDatasPorDataReferenciaAsync(string anoMes, CancellationToken cancellationToke)
        {

            if (!String.IsNullOrEmpty(anoMes))
            {
                var calendarioEntry = await _calendarioSistemaRepository.BuscaDatasPorDataReferenciaAsync(anoMes, cancellationToke);

                if (calendarioEntry != null)
                {
                    var valorParametroSistemaEntry = await _valorParametroSistemaRepository
                        .GetAsync(p => p.ParametroSistema.NomeParametro != ApplicationConstants.ParaMetroDiaUtilOuCorrido
                        && p.ParametroSistema.NomeParametro ==
                        ApplicationConstants.ParametroAvisoTerminoPeriodoAnaliseContestacao, cancellationToke);

                    var dataAvisoTerminoPeriodo = calendarioEntry.DataTerminoPeriodoAnaliseContestacoes.AddDays(-Int32.Parse(valorParametroSistemaEntry.FirstOrDefault().ValParametro)).ToString(ApplicationConstants.DateFormat);


                    return new CalendarioFullViewModel
                    {
                        DataAvisoTerminoPeriodo = dataAvisoTerminoPeriodo,
                        Data = calendarioEntry.DataLiberacaoContestacoesAgente.ToString(ApplicationConstants.DateFormat),
                        DataTerminoPeriodoContestacoesAgente = calendarioEntry.DataTerminoPeriodoContestacoesAgente.ToString(ApplicationConstants.DateFormat),
                        DataTerminoPeriodoAnaliseContestacoes = calendarioEntry.DataTerminoPeriodoAnaliseContestacoes.ToString(ApplicationConstants.DateFormat),
                        DataReferenciaIndicador = calendarioEntry.DataReferenciaIndicador
                    };
                }

            }

            return null;
        }

        public async Task<IEnumerable<string>> DatasDisponiveisParaDownloadAsync(CancellationToken cancellationToke)
        {
            return await _relatorioRepository
                .BuscarDatasDownloadAsync(cancellationToke);
        }

        public async Task<bool> GerarCalendarioAsync(string anoMes, CancellationToken cancellationToken)
        {
            var calendarios = _calendarioSistemaRepository.PegarTodosCalendario();

            if (calendarios.Any(c => c.DataReferenciaIndicador == anoMes.ConvertToAnomeReferencia()))
            {
                throw new Exception($"A operação para o período {anoMes} já foi processada e não pode ser repetida.");
            }

            var localidades = Enum.GetValues(typeof(CentroOperacaoEnum)).OfType<CentroOperacaoEnum>().Select(x => x.ToString()).ToList();

            var localidadesExistentes = await _calendarioSistemaRepository.VerificarLocalidadeExistente(anoMes.ConvertToAnomeReferencia(), cancellationToken);

            if (!localidadesExistentes.Except(localidades).Any())
            {
                throw new Exception($"Não existem dados para o período {anoMes} a serem processados.");
            }

            DateTime dataReferenciaIndicador = anoMes.ConvertToAnomeReferencia().ConvertStringToDate();
            var calendarioParaSerAdicionado = _calendarioCalculator.CalculaCalendarioDoSistema();

            calendarioParaSerAdicionado.DataReferenciaIndicador = anoMes.ConvertToAnomeReferencia();

            if (_calendarioSistemaRepository.AdicionarRegistro(calendarioParaSerAdicionado))
            {
                _cargaService.GerarRelatorio(new ProcessamentoCargaFilterViewModel
                {
                    AnoMes = new List<string> { anoMes.ConvertToAnomeReferencia() },
                    ProcRelatorioTipo = "all",
                });

                return true;
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

            }
        }

        private async void ProcessarRelatorio(string mesAno, List<Agente> agenteSubset, List<TpRelatorio> tipoRelatorio, List<TpIndicador> indicadores, CargaRepository cargaRepository, IRelatorioService relatorioService, IGrandezaRepository grandezaRepository, List<Relatorio> groupRelatorio)
        {
            var agenteFilter = agenteSubset.DistinctBy(c => c.AgeMrid).OrderBy(c => c.NomeCurto).ToList();

            MensagemProcessamento = $"LOG#Agente: {agenteFilter.Count()} - Relatorio: ${tipoRelatorio.Count()} Início do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
            log.Info($"[Agente: {agenteFilter.Count()} Relatorio: ${tipoRelatorio.Count()}]Início do processamento: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");

            foreach (var agente in agenteFilter)
            {
                var automacao = new AutomacaoIndicador
                {
                    EntryAgentes = cargaRepository
                    .BuscarAgente(mesAno, agente.AgeMrid).Cast<object>().ToList(),
                    EntryResultadoIndicador = cargaRepository
                    .BuscarSSCL(mesAno, agente.AgeMrid).Cast<object>().ToList(),
                    EntryInstalacao = cargaRepository
                    .BuscarInstalacaoRecurso(mesAno, agente.AgeMrid).Cast<object>().ToList(),
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
                                ConfigurarAutomacao(automacao, relatorio.Codigo, agente.IdOns, mesAno);
                                relatorioService.GerarRMCViolacoesQualidade(automacao);
                            }
                            else if (relatorio.Codigo == nameof(TipoRelatorioEnum.RAcDQ) && !existe)
                            {
                                ConfigurarAutomacao(automacao, relatorio.Codigo, agente.IdOns, mesAno);
                                relatorioService.GerarRMCAcompanhamentoQualidade(automacao);
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
    }
}
