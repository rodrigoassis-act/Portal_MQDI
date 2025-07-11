using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Response;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Constants;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Shared.Settings;
using ONS.PortalMQDI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class SupervisaoTempoRealService : ISupervisaoTempoRealService
    {
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly IOptions<ServiceGlobalSettings> _settings;
        private readonly ILogEventoService _logEventoService;
        public SupervisaoTempoRealService(IResultadoIndicadorRepository resultadoIndicadorRepository, IOptions<ServiceGlobalSettings> settings, ILogEventoService logEventoService)
        {
            this._resultadoIndicadorRepository = resultadoIndicadorRepository;
            this._settings = settings;
            this._logEventoService = logEventoService;
        }

        public async Task<IEnumerable<SupervisaoTempoRealViewModel>> BuscarAsync(SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken)
        {
            var isProprietario = request.CodScada.Contains('*');

            await _logEventoService.RegistrarEventoAsync(request.CodAgente, request.DataProcessar.ConvertToAnomeReferencia(), PageEnum.SupervisaoEmTempoReal, cancellationToken);

            var serviceRequest = BuscaListaCepel(request.CodScada.Trim('*'));

            var listaGrandeza = await _resultadoIndicadorRepository.SupervisaoTempoResultadoIndicador(request.CodAgente, request.DataProcessar, cancellationToken);

            if ((listaGrandeza != null && listaGrandeza.Count() > 0) && serviceRequest != null && serviceRequest.Count() > 0)
            {
                if (isProprietario)
                {
                    var model = serviceRequest.Select(x => new SupervisaoTempoRealViewModel()
                    {
                        tipo = x.tipo.Equals(ApplicationConstants.EqualAnalogico) ? ApplicationConstants.TextAnalogico : ApplicationConstants.TextDigital,
                        falha = x.falha,
                        id = x.id,
                        nome = x.nome,
                        valaq = x.valaq,
                        enderecoProtocolo = listaGrandeza.FirstOrDefault(c => c.CodGrandeza == x.id)?.EnderecoProtocolo,
                        mqdi = String.IsNullOrEmpty(listaGrandeza.FirstOrDefault(c => c.CodGrandeza == x.id)?.EnderecoProtocolo) ? "Não" : "Sim"
                    }).ToList();

                    return model;
                }
                else
                {
                    var model = serviceRequest.Where(c => listaGrandeza.Select(x => x.CodGrandeza).Contains(c.id)).Select(x => new SupervisaoTempoRealViewModel()
                    {
                        tipo = x.tipo.Equals(ApplicationConstants.EqualAnalogico) ? ApplicationConstants.TextAnalogico : ApplicationConstants.TextDigital,
                        falha = x.falha,
                        id = x.id,
                        nome = x.nome,
                        valaq = x.valaq,
                        enderecoProtocolo = listaGrandeza.FirstOrDefault(c => c.CodGrandeza == x.id)?.EnderecoProtocolo,
                        mqdi = "Sim"
                    }).ToList();

                    return model;
                }
            }
            return new List<SupervisaoTempoRealViewModel>();
        }

        private List<SupervisaoTempoRealViewModel> BuscaListaCepel(string scada)
        {
            try
            {
                var http = new HTTPUtil();
                var uri = $"{_settings.Value.ServiceCEPEL}?id={scada}";
                var response = http.Get<SupervisaoTempoRealResponse>(uri);
                return response.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"(Serviço Cepel)- {ex.Message}");
            }
        }

        public async Task<IEnumerable<ScadaView>> ListaScadaAsync(SupervisaoTempoRealFiltroViewModel request, CancellationToken cancellationToken)
        {
            return await _resultadoIndicadorRepository.FiltrarAgentesScadaAsync(request.DataProcessar, request.CodAgente, cancellationToken);
        }

        public string BuscarDatas()
        {
            var data = _resultadoIndicadorRepository.BuscarDataReferenciaMaisRecente();

            return data.Replace('-', '/');
        }
    }
}
