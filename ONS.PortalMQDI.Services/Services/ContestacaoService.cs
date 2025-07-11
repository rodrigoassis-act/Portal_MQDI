using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class ContestacaoService : IContestacaoService
    {
        private readonly IContestacaoRepository _contestacaoRepository;
        private readonly IUserService _userService;
        private readonly ICalendarioSistemaRepository _calendarioSistemaRepository;
        private readonly IAgenteRepository _agenteRepository;
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly IInstalacaoRepository _instalacaoRepository;
        private readonly IResultadoContestacaoRepository _resultadoContestacaoRepository;
        private readonly NotificacaoService _notificacaoService;
        private readonly ILogEventoService _logEventoService;

        public ContestacaoService(IContestacaoRepository contestacaoRepository, IUserService userService,
            ICalendarioSistemaRepository calendarioSistemaRepository,
            IAgenteRepository agenteRepository,
            IResultadoIndicadorRepository resultadoIndicadorRepository, IInstalacaoRepository instalacaoRepository,
            IResultadoContestacaoRepository resultadoContestacaoRepository,
            NotificacaoService notificacaoService, ILogEventoService logEventoService)
        {
            _contestacaoRepository = contestacaoRepository;
            _userService = userService;
            _calendarioSistemaRepository = calendarioSistemaRepository;
            _agenteRepository = agenteRepository;
            _resultadoIndicadorRepository = resultadoIndicadorRepository;
            _instalacaoRepository = instalacaoRepository;
            _resultadoContestacaoRepository = resultadoContestacaoRepository;
            _notificacaoService = notificacaoService;
            _logEventoService = logEventoService;
        }

        public async Task<bool> AutorizacaoContestacaoAsync(string mesAno, CancellationToken cancellationToke)
        {
            var possuiPermissao = _userService.CheckOperation(PermissionEnum.MQDICriarContestacao);
            var calendarioAtual = await _calendarioSistemaRepository.BuscaDatasPorDataReferenciaAsync(mesAno, cancellationToke);

            if (calendarioAtual != null && DateTime.Now.Date >= calendarioAtual.DataLiberacaoContestacoesAgente.Date && DateTime.Now.Date
                <= calendarioAtual.DataTerminoPeriodoContestacoesAgente.Date)
            {
                return possuiPermissao;
            }

            return false;
        }

        public async Task<bool> AutorizacaoResponderContestacaoAsync(string mesAno, CancellationToken cancellationToke)
        {
            var possuiPermissao = _userService.CheckOperation(PermissionEnum.MQDIResponderContestacao);
            var calendarioAtual = await _calendarioSistemaRepository.BuscaDatasPorDataReferenciaAsync(mesAno, cancellationToke);

            if (calendarioAtual != null && DateTime.Now.Date >= calendarioAtual.DataLiberacaoContestacoesAgente.Date && DateTime.Now.Date
                <= calendarioAtual.DataTerminoPeriodoContestacoesAgente.Date)
            {
                return possuiPermissao;
            }

            return false;
        }

        public async Task<List<SelectItemViewModel<string>>> BuscarAgenteAsync(string mesAno, CancellationToken cancellationToke)
        {
            var agentesEntry = await _contestacaoRepository.BuscarAgenteAsync(mesAno, cancellationToke);


            var temp = agentesEntry.GroupBy(c => c.AgeMrid);

            return agentesEntry.Select(x => new SelectItemViewModel<string>
            {
                Label = x.NomeCurto.Trim(),
                Value = x.AgeMrid.Trim(),
                Title = x.NomeLongo.Trim(),
                Id = x.AnoMes
            }).ToList();
        }

        public async Task<ContestacaoAnalistaViewModel> CriarContestacaoAsync(ContestacaoAnalistaViewModel contestacaoViewModel, CancellationToken cancellationToke)
        {
            if (contestacaoViewModel.IdConstestacao.HasValue)
            {
                if (_userService.IsAdministrator())
                {
                    var resultadoConstetacao = await _resultadoContestacaoRepository.FirstItemAsync(c => c.IdContestacao == contestacaoViewModel.IdConstestacao.Value, cancellationToke);

                    if (resultadoConstetacao != null)
                    {
                        resultadoConstetacao.IdRequisito = contestacaoViewModel.TipoStatus.Value ? 1 : 2;
                        resultadoConstetacao.IdContestacao = contestacaoViewModel.IdConstestacao.Value;
                        resultadoConstetacao.DataResultado = DateTime.Now;
                        resultadoConstetacao.LoginAnalista = _userService.LoginUsuario();
                        resultadoConstetacao.Observacao = contestacaoViewModel.ComentarioOns;


                        await _resultadoContestacaoRepository.UpdateAsync(resultadoConstetacao.IdResultadoContestacao, resultadoConstetacao, cancellationToke);
                    }
                    else
                    {
                        var resultadoContestacaoEntry = new ResultadoContestacao
                        {
                            IdRequisito = contestacaoViewModel.TipoStatus.Value ? 1 : 2,
                            IdContestacao = contestacaoViewModel.IdConstestacao.Value,
                            DataResultado = DateTime.Now,
                            LoginAnalista = _userService.LoginUsuario(),
                            Observacao = contestacaoViewModel.ComentarioOns
                        };

                        var result = await _resultadoContestacaoRepository.InsertAsync(resultadoContestacaoEntry, cancellationToke);
                    }
                }
                else
                {
                    var contestacaoEntry = await _contestacaoRepository.FirstItemAsync(c => c.IdResultadoIndicador == contestacaoViewModel.IdConstestacao.Value, cancellationToke);

                    if (contestacaoEntry != null)
                    {
                        contestacaoEntry.DescricaoContestacao = contestacaoViewModel.ComentarioAnalista;
                        contestacaoEntry.IdResultadoIndicador = contestacaoViewModel.IdResultadoIndicador;
                        contestacaoEntry.LoginUsuarioAgente = _userService.LoginUsuario();

                        await _contestacaoRepository.UpdateAsync(contestacaoEntry.IdContestacao, contestacaoEntry, cancellationToke);
                    }
                }
            }
            else
            {
                var contestacaoEntry = await _contestacaoRepository.FirstItemAsync(c => c.IdResultadoIndicador == contestacaoViewModel.IdResultadoIndicador, cancellationToke);

                if (contestacaoEntry != null)
                {
                    contestacaoEntry.DescricaoContestacao = contestacaoViewModel.ComentarioAnalista;
                    contestacaoEntry.IdResultadoIndicador = contestacaoViewModel.IdResultadoIndicador;
                    contestacaoEntry.LoginUsuarioAgente = _userService.LoginUsuario();

                    await _contestacaoRepository.UpdateAsync(contestacaoEntry.IdContestacao, contestacaoEntry, cancellationToke);
                }
                else
                {
                    var contestacao = new Contestacao
                    {
                        DescricaoContestacao = contestacaoViewModel.ComentarioAnalista,
                        IdResultadoIndicador = contestacaoViewModel.IdResultadoIndicador,
                        LoginUsuarioAgente = _userService.LoginUsuario()
                    };

                    var result = await _contestacaoRepository.InsertAsync(contestacao, cancellationToke);
                }
            }

            return contestacaoViewModel;
        }

        public async Task<List<string>> DataDisponivelAsync(CancellationToken cancellationToke)
        {
            var constestacao = await _contestacaoRepository.DataDisponivelAsync(cancellationToke);
            return constestacao.Select(c => c.Replace("-", "/")).ToList();
        }

        public async Task<ResultadoIndicadorViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            var resultadoIndicadorViewModel = new ResultadoIndicadorViewModel();

            var entryAgentes = await _agenteRepository
                .BuscarViewPorFiltroAsync(filtro.MesAno, filtro.Agente, filtro.TpIndicador, filtro.FlgViolacaoAnual, true, cancellationToke);

            var entryResultadoIndicador = await _resultadoIndicadorRepository
                .BuscarViewPorFiltroAsync(filtro.MesAno, filtro.Agente, filtro.TpIndicador, filtro.FlgViolacaoAnual, true, cancellationToke);

            var entryInstalacao = await _instalacaoRepository
                .BuscarInstalacaoRecusoroCostentacaoViewAsync(filtro.MesAno, new List<string> { filtro.Agente }, filtro.TpIndicador, filtro.FlgViolacaoAnual, cancellationToke);

            resultadoIndicadorViewModel.Instalacao = MontarTableInstalacao(entryInstalacao.ToList());
            resultadoIndicadorViewModel.Agente = MontarTableAgente(entryAgentes.ToList());
            resultadoIndicadorViewModel.SSCL = MontarTableSSCL(entryResultadoIndicador.ToList());

            await _logEventoService.RegistrarEventoAsync(filtro.Agente, filtro.MesAno, PageEnum.ConsultaDeContestacoes, cancellationToke);

            return resultadoIndicadorViewModel;
        }


        private List<ResultadoIndicadorTableViewModel> MontarTableAgente(List<ConsultaIndicadorAgenteView> entryAgente)
        {
            var temp = new List<ResultadoIndicadorTableViewModel>();

            var agenteGrouped = entryAgente.GroupBy(i => new { i.NomeCurto, i.CodIndicador });

            foreach (var agenteGroup in agenteGrouped)
            {

                var tableRow = new ResultadoIndicadorTableViewModel
                {
                    ValueName = agenteGroup.Key.NomeCurto,
                    Indicador = agenteGroup.Key.CodIndicador,
                };

                foreach (var item in agenteGroup)
                {
                    tableRow.DscAnalista = item.DscAnalistaConstestacao;
                    tableRow.DscAnalistaONS = item.DscOnsConstestacao;

                    tableRow.IdResultadoIndicador = item.IdResultadoIndicador;

                    tableRow.Mensal.Valor = item.ValMensal.RoundToTwoDecimalPlaces();
                    tableRow.Mensal.Violacao = item.FlgViolacaoMensal == false ? true : false;

                    tableRow.Anual.Valor = item.ValAnual.RoundToTwoDecimalPlaces();
                    tableRow.Anual.Violacao = item.FlgViolacaoAnual == false ? true : false;
                    tableRow.IdConstestacao = item.IdConstestacao;
                    tableRow.ConstestacaoStatus = item.ConstestacaoStatus == 1 ? true : false;
                }

                temp.Add(tableRow);
            }

            return temp.OrderBy(c => c.Cos).ThenBy(c => c.ValueName).ToList();
        }

        private List<ResultadoIndicadorTableViewModel> MontarTableSSCL(List<ConsultaIndicadorSSCLView> entryResultado)
        {
            var temp = new List<ResultadoIndicadorTableViewModel>();

            var resultadoGrouped = entryResultado.GroupBy(r => new { r.UtrCd, r.CodIndicador, r.CosId });

            foreach (var resultadoGroup in resultadoGrouped)
            {
                var cosIdWithoutPrefix = resultadoGroup.Key.CosId.Replace("COSR-", "");
                if (Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum))
                {
                    var cosDescription = cosIdEnum.GetDescription();

                    var tableRow = new ResultadoIndicadorTableViewModel
                    {
                        ValueName = resultadoGroup.Key.UtrCd,
                        Indicador = resultadoGroup.Key.CodIndicador,
                        Cos = cosDescription,
                    };


                    foreach (var item in resultadoGroup)
                    {
                        tableRow.DscAnalista = item.DscAnalistaConstestacao;
                        tableRow.DscAnalistaONS = item.DscOnsConstestacao;

                        tableRow.IdResultadoIndicador = item.IdResultadoIndicador;

                        tableRow.Mensal.Valor = item.ValMensal.RoundToTwoDecimalPlaces();
                        tableRow.Mensal.Violacao = item.FlgViolacaoMensal == false ? true : false;

                        tableRow.Anual.Valor = item.ValAnual.RoundToTwoDecimalPlaces();
                        tableRow.Anual.Violacao = item.FlgViolacaoAnual == false ? true : false;
                        tableRow.IdConstestacao = item.IdConstestacao;

                        tableRow.ConstestacaoStatus = item.ConstestacaoStatus == 1 ? true : false;
                    }

                    temp.Add(tableRow);
                }
            }

            return temp.OrderBy(c => c.Cos).ToList();
        }

        public List<ResultadoIndicadorTableViewModel> MontarTableInstalacao(List<ContestacaoInstalacaoRecursoView> entryInstalacao)
        {
            var temp = new List<ResultadoIndicadorTableViewModel>();

            var groupInstalacao = entryInstalacao.GroupBy(r => new { r.InsMrid, r.NomCurto });

            foreach (var item in groupInstalacao)
            {
                var instalacao = item.First();
                var cosIdWithoutPrefix = instalacao.CosId.Replace("COSR-", "");

                if (Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum))
                {
                    var tableRow = new ResultadoIndicadorTableViewModel
                    {
                        ValueName = instalacao.NomCurto,
                        Indicador = instalacao.CodIndicador,
                        Cos = cosIdEnum.GetDescription(),
                        IdCos = instalacao.CosId,
                        DscAnalista = instalacao.InstalacaoAnalistaContestacao,
                        DscAnalistaONS = instalacao.InstalacaoOnsContestacao,
                        IdResultadoIndicador = instalacao.IdResultadoIndicador,
                        IdInstalacao = instalacao.InsMrid,
                        IdIncador = instalacao.IdTipoIndicador,
                        IdConstestacao = instalacao.InstalacaoOnsIdContestacao,
                        ConstestacaoStatus = instalacao.InstalacaoOnsStatus == 1 ? true : false,
                    };

                    tableRow.Mensal.Valor = instalacao.ValMensal.RoundToTwoDecimalPlaces();
                    tableRow.Mensal.Violacao = instalacao.FlgViolacaoMensal == false ? true : false;
                    tableRow.Anual.Valor = instalacao.ValAnual.RoundToTwoDecimalPlaces();
                    tableRow.Anual.Violacao = instalacao.FlgViolacaoAnual == false ? true : false;

                    foreach (var recurso in item.Where(c => c.RecursoAnalistaContestacao != null))
                    {
                        Enum.TryParse(recurso.Tprede, out TipoRedeEnum tipoRedeEnum);

                        var recursoRow = new ItemRecusoTableViewModel()
                        {
                            CodOns = recurso.IdoOns,
                            Indicador = recurso.CodIndicador,
                            Descricao = recurso.DscGrandeza,
                        };

                        recursoRow.Mensal.Valor = recurso.RecursoValorMensal.Value.RoundToTwoDecimalPlaces();
                        recursoRow.Mensal.Violacao = recurso.RecursoFragMensal == false ? true : false;
                        recursoRow.Anual.Valor = recurso.RecursoValorAnual.Value.RoundToTwoDecimalPlaces();
                        recursoRow.Anual.Violacao = recurso.RecursoFragAnual == false ? true : false;

                        recursoRow.CostentacaoAgente = recurso.RecursoAnalistaContestacao;
                        recursoRow.CostentacaoOns = recurso.RecursoOnsContestacao;
                        recursoRow.ConstestacaoStatus = recurso.RecursoOnsStatus == 1 ? true : false;
                        recursoRow.IdConstatacao = recurso.RecursoOnsIdContestacao.Value;
                        recursoRow.Rede = tipoRedeEnum.GetDescription();
                        recursoRow.CodLscinf = recurso.CodLscinf;
                        recursoRow.NomeFisico = recurso.NomEnderecoFisico;
                        recursoRow.IdResultadoIndicador = recurso.RecursoIdResultadoIndicador.Value;

                        tableRow.Recurso.Add(recursoRow);
                    }

                    temp.Add(tableRow);
                }
            }


            return temp.OrderBy(c => c.Cos).ThenBy(c => c.ValueName).ToList();
        }
    }
}
