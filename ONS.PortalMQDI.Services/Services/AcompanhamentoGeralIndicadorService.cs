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
    public class AcompanhamentoGeralIndicadorService : IAcompanhamentoGeralIndicadorService
    {
        private readonly IAgenteRepository _agenteRepository;
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly IInstalacaoRepository _instalacaoRepository;
        private readonly ILogEventoService _logEventoService;
        public AcompanhamentoGeralIndicadorService(IAgenteRepository agenteRepository, 
            IResultadoIndicadorRepository resultadoIndicadorRepository, 
            IInstalacaoRepository instalacaoRepository, ILogEventoService logEventoService)
        {
            _resultadoIndicadorRepository = resultadoIndicadorRepository;
            _agenteRepository = agenteRepository;
            _instalacaoRepository = instalacaoRepository;
            _logEventoService = logEventoService;
        }

        public async Task<AcompanhamentoIndicadoresViewModel> FiltroAsync(AcompanhamentoIndicadoresFiltroViewModel filtro, CancellationToken cancellationToken)
        {
            var viewModel = new AcompanhamentoIndicadoresViewModel();
            var datas = filtro.MesAno.GeneratePastMonths(12);
            var datasFormat = datas.Select(data => data.ToString("yyyy-MM")).ToList();

            var entryAgentes = await _agenteRepository.BuscarViewPorFiltroAsync(datas, filtro.Agente, filtro.TpIndicador, null, cancellationToken);
            var entryResultadoIndicador = await _resultadoIndicadorRepository.BuscarViewPorFiltroAsync(datas, filtro.Agente, filtro.TpIndicador, null, cancellationToken);
            var entryInstalacao = await _instalacaoRepository.BuscarViewPorFiltroAsync(datas, filtro.Agente, filtro.TpIndicador, null, cancellationToken);


            if (entryAgentes == null && entryResultadoIndicador == null && entryInstalacao == null)
            {
                return null;
            }

            viewModel.Agente = MontarTableAgente(entryAgentes.ToList(), datasFormat);
            viewModel.ResultadoIndicador = MontarTableSSCL(entryResultadoIndicador.ToList(), datasFormat);
            viewModel.Instalacao = MontarTableInstalacao(entryInstalacao.ToList(), datasFormat);
            viewModel.DisplayColumns = datas.OrderBy(c => c).Select(data => data.ToString("MM/yyyy")).ToList();

            // Registrar Evento
            await _logEventoService.RegistrarEventoAsync(filtro.Agente, filtro.MesAno, PageEnum.AcompanhamentoGeralDeIndicadores, cancellationToken);

            return viewModel;
        }



        public List<AgenteTableViewModel> MontarTableAgente(List<AgenteIndicadorView> entryAgente, List<string> datas)
        {
            var temp = new List<AgenteTableViewModel>();

            var agentesGrouped = entryAgente.GroupBy(a => new { a.NomeCurto, a.CodIndicador });

            foreach (var agenteGroup in agentesGrouped)
            {
                var tableRow = new AgenteTableViewModel
                {
                    Agente = agenteGroup.Key.NomeCurto,
                    Indicador = agenteGroup.Key.CodIndicador,
                    Meses = new Dictionary<string, AgenteItemViewModel>()
                };

                foreach (var data in datas)
                {
                    var agenteMes = agenteGroup.FirstOrDefault(a => a.AnoMesReferencia == data);

                    var indicadorValor = new AgenteItemViewModel();

                    if (agenteMes != null)
                    {
                        indicadorValor.Valor = agenteMes.ValMensal.RoundToTwoDecimalPlaces();
                        indicadorValor.Violacao = agenteMes.FlgViolacaoMensal == false ? true : false;
                    }
                    else
                    {
                        indicadorValor.Valor = "0.0";
                        indicadorValor.Violacao = false;
                    }

                    tableRow.Meses[data] = indicadorValor;
                }

                temp.Add(tableRow);
            }

            return temp;
        }


        public List<ResultadoTableViewModel> MontarTableSSCL(List<ResultadoIndicadorView> entryResultado, List<string> datas)
        {
            var temp = new List<ResultadoTableViewModel>();

            var resultadoGrouped = entryResultado.GroupBy(r => new { r.UtrCd, r.CodIndicador, r.CosId });

            foreach (var resultadoGroup in resultadoGrouped)
            {
                var cosIdWithoutPrefix = resultadoGroup.Key.CosId.Replace("COSR-", "");
                if (Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum))
                {
                    var cosDescription = cosIdEnum.GetDescription();

                    var tableRow = new ResultadoTableViewModel
                    {
                        SSCLCD = resultadoGroup.Key.UtrCd,
                        Indicador = resultadoGroup.Key.CodIndicador,
                        Cos = cosDescription,
                        Meses = new Dictionary<string, ResultadoItemViewModel>()
                    };

                    foreach (var data in datas)
                    {
                        var resultadoMes = resultadoGroup.FirstOrDefault(r => r.AnoMesReferencia == data);

                        var indicadorValor = new ResultadoItemViewModel();

                        if (resultadoMes != null)
                        {
                            indicadorValor.Valor = resultadoMes.ValMensal.RoundToTwoDecimalPlaces();
                            indicadorValor.Violacao = resultadoMes.FlgViolacaoMensal == false ? true : false;
                        }
                        else
                        {
                            indicadorValor.Valor = "0";
                            indicadorValor.Violacao = false;
                        }

                        tableRow.Meses[data] = indicadorValor;
                    }

                    temp.Add(tableRow);
                }
            }

            return temp.OrderBy(c => c.Cos).ToList();
        }


        public List<InstalacaoTableViewModel> MontarTableInstalacao(List<InstalacaoView> entryInstalacao, List<string> datas)
        {
            var temp = new List<InstalacaoTableViewModel>();

            var instalacaoGrouped = entryInstalacao.GroupBy(i => new { i.NomCurto, i.CodTpIndicador, i.CosId });

            foreach (var instalacaoGroup in instalacaoGrouped)
            {
                var cosIdWithoutPrefix = instalacaoGroup.Key.CosId.Replace("COSR-", "");
                if (Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum))
                {
                    var cosDescription = cosIdEnum.GetDescription();

                    var tableRow = new InstalacaoTableViewModel
                    {
                        Instalacao = instalacaoGroup.Key.NomCurto,
                        Indicador = instalacaoGroup.Key.CodTpIndicador,
                        Cos = cosDescription,
                        Meses = new Dictionary<string, InstalacaoItemViewModel>()
                    };

                    foreach (var data in datas)
                    {
                        var instalacaoMes = instalacaoGroup.FirstOrDefault(i => i.AnoMesReferencia == data);

                        var indicadorValor = new InstalacaoItemViewModel();

                        if (instalacaoMes != null)
                        {
                            indicadorValor.Valor = instalacaoMes.ValMensal.RoundToTwoDecimalPlaces();
                            indicadorValor.Violacao = instalacaoMes.FlgViolacaoMensal == false ? true : false;
                        }
                        else
                        {
                            indicadorValor.Valor = "0.0";
                            indicadorValor.Violacao = false;
                        }

                        tableRow.Meses[data] = indicadorValor;
                    }

                    temp.Add(tableRow);
                }
            }

            return temp.OrderBy(c => c.Cos).ThenBy(c => c.Instalacao).ToList();
        }
    }
}
