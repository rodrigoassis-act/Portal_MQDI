using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace ONS.PortalMQDI.Services.Services
{
    public class ResultadoDiarioService : IResultadoDiarioService
    {
        private readonly IResultadoDiarioRepository _resultadoDiarioRepository;
        private readonly ILogEventoService _logEventoService;
        public ResultadoDiarioService(IResultadoDiarioRepository resultadoDiarioRepository, ILogEventoService logEventoService)
        {
            _resultadoDiarioRepository = resultadoDiarioRepository;
            _logEventoService = logEventoService;
        }

        public async Task<List<ResultadoDiarioViewModel>> BuscarResultadoDiarioAsync(string data, string agente, string tpIndicador, CancellationToken cancellationToken)
        {
            string idIndicador = "";
            string idRecurso = "";


            await _logEventoService.RegistrarEventoAsync(agente, null, PageEnum.ConsultaDeIndicadoresDiario, cancellationToken);

            if (tpIndicador == "DRSC")
            {
                var resList = await _resultadoDiarioRepository.BuscarResultadoDiarioDRSCAsync(data.ConvertStringToDateString(), agente, cancellationToken);

                return resList.Select(c => new ResultadoDiarioViewModel
                {
                    Descricao = c.Descricao,
                    DispDiaria = c.DispDiaria.RoundToTwoDecimalPlaces(),
                    EnderecoProtocolo = c.EnderecoProtocolo,
                    FlgDispDiario = c.FlgDispDiario > 0 ? true : false,
                    IdoOns = c.IdoOns,
                    Instalacao = c.Instalacao,
                    Lscc = c.Lscc,
                    Rede = c.Rede
                }).ToList();
            }
            else if (tpIndicador == "DCD")
            {
                var resList = await _resultadoDiarioRepository.BuscarResultadoDiarioDCDAsync(data.ConvertStringToDateString(), agente, cancellationToken);
                return resList.Select(c => this.ResultadoDiarioDCDView(c)).ToList();
            }

            return new List<ResultadoDiarioViewModel> { new ResultadoDiarioViewModel { } };
        }


        public ResultadoDiarioViewModel ResultadoDiarioDCDView(ResultadoDiarioDCDView item)
        {
            Enum.TryParse(item.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);

            var temp = new ResultadoDiarioViewModel();
            temp.FlgDispDiario = item.FlgDispDiario > 0 ? true : false;
            temp.DispDiaria = item.ValDispDiario.RoundToTwoDecimalPlaces();
            temp.Lscinf = item.CodLscinf;
            temp.UtrCd = item.UtrCd;
            temp.Indicador = item.CodTpIndicador;
            temp.Centro = cosIdEnum.GetDescription();
            return temp;
        }
    }
}