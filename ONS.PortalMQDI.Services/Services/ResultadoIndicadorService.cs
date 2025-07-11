using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class ResultadoIndicadorService : IResultadoIndicadorService
    {
        private readonly IAgenteRepository _agenteRepository;
        private readonly ITpIndicadorRepository _tpIndicadorRepository;
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly IInstalacaoRepository _instalacaoRepository;
        private readonly ILogEventoService _logEventoService;
        public ResultadoIndicadorService(ITpIndicadorRepository tpIndicadorRepository,
            IAgenteRepository agenteRepository,
            IResultadoIndicadorRepository resultadoIndicadorRepository,
            IInstalacaoRepository instalacaoRepository,
            ILogEventoService logEventoService)
        {
            _tpIndicadorRepository = tpIndicadorRepository;
            _agenteRepository = agenteRepository;
            _resultadoIndicadorRepository = resultadoIndicadorRepository;
            _instalacaoRepository = instalacaoRepository;
            _logEventoService = logEventoService;
        }

        public async Task<List<SelectItemViewModel<string>>> ListaTpIndicadorAsync(CancellationToken cancellationToke)
        {
            var query = await _tpIndicadorRepository.GetAllAsync(cancellationToke);

            return query.Where(c=> c.Id != 0).Select(x => new SelectItemViewModel<string>
            {
                Id = x.Id.ToString(),
                Value = x.CodIndicador,
                Label = x.CodIndicador,
                Title = x.Nome,
            }).ToList();
        }

        public async Task<ResultadoIndicadorViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToke, bool isContestacao = false)
        {
            var resultadoIndicadorViewModel = new ResultadoIndicadorViewModel();

            var entryAgentes = await _agenteRepository
                .BuscarViewPorFiltroAsync(filtro.MesAno, filtro.Agente, filtro.TpIndicador, null, isContestacao, cancellationToke);

            var entryResultadoIndicador = await _resultadoIndicadorRepository
                .BuscarViewPorFiltroAsync(filtro.MesAno, filtro.Agente, filtro.TpIndicador, null, isContestacao, cancellationToke);

            var entryInstalacao = await _instalacaoRepository
                .BuscarViewPorFiltroAsync(filtro.MesAno, filtro.Agente, filtro.TpIndicador, null, isContestacao, cancellationToke);


            resultadoIndicadorViewModel.Agente = MontarTableAgente(entryAgentes.ToList());
            resultadoIndicadorViewModel.SSCL = MontarTableSSCL(entryResultadoIndicador.ToList());
            resultadoIndicadorViewModel.Instalacao = MontarTableInstalacao(entryInstalacao.ToList());

            await _logEventoService.RegistrarEventoAsync(filtro.Agente, filtro.MesAno, PageEnum.ConsultaDeIndicadores, cancellationToke);

            return resultadoIndicadorViewModel;
        }


        #region Montar Tabelas
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
                    }

                    temp.Add(tableRow);
                }
            }

            return temp.OrderBy(c => c.Cos).ToList();
        }

        public List<ResultadoIndicadorTableViewModel> MontarTableInstalacao(List<ConsultaIndicadorInstalacaoView> entryInstalacao)
        {
            var temp = new List<ResultadoIndicadorTableViewModel>();

            var instalacaoGrouped = entryInstalacao.GroupBy(i => new { i.NomCurto, i.CodTpIndicador, i.CosId });

            foreach (var instalacaoGroup in instalacaoGrouped)
            {
                var cosIdWithoutPrefix = instalacaoGroup.Key.CosId.Replace("COSR-", "");
                if (Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum))
                {
                    var cosDescription = cosIdEnum.GetDescription();


                    var tableRow = new ResultadoIndicadorTableViewModel
                    {
                        ValueName = instalacaoGroup.Key.NomCurto,
                        Indicador = instalacaoGroup.Key.CodTpIndicador,
                        Cos = cosDescription,
                        IdCos = instalacaoGroup.Key.CosId,
                    };


                    foreach (var item in instalacaoGroup)
                    {
                        tableRow.DscAnalista = item.DscAnalistaConstestacao;
                        tableRow.DscAnalistaONS = item.DscOnsConstestacao;
                        tableRow.IdResultadoIndicador = item.IdResultadoIndicador;
                        tableRow.Mensal.Valor = item.ValMensal.RoundToTwoDecimalPlaces();
                        tableRow.Mensal.Violacao = item.FlgViolacaoMensal == false ? true : false;
                        tableRow.Anual.Valor = item.ValAnual.RoundToTwoDecimalPlaces();
                        tableRow.Anual.Violacao = item.FlgViolacaoAnual == false ? true : false;
                        tableRow.IdInstalacao = item.IdInstalacao;
                        tableRow.IdIncador = item.IdTpIndicador;
                    }

                    temp.Add(tableRow);
                }
            }

            return temp.OrderBy(c => c.Cos).ThenBy(c => c.ValueName).ToList();
        }

        public async Task<List<SgiViewModel>> BuscarSgiAsync(int idResultadoIndicador, CancellationToken cancellationToke)
        {
            var sgiView = await _resultadoIndicadorRepository.BuscarSgiViewAsync(idResultadoIndicador, cancellationToke);
            return sgiView.Select(c => new SgiViewModel
            {
                SgiInicioefetivo = c.SgiInicioefetivo.Value.ToString("dd/MM/yyyy"),
                SgiNumONS = c.SgiNumONS,
                SgiNumSequenciaSgi = c.SgiNumSequenciaSgi.Value,
                SgiTerminoEfetivo = c.SgiTerminoEfetivo.Value.ToString("dd/MM/yyyy")

            }).ToList();
        }
        #endregion
    }
}
