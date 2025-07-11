using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using ONS.PortalMQDI.Shared.Settings;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Tests.Services
{
    public class SupervisaoTempoRealServiceTests
    {
        private Mock<IResultadoIndicadorRepository> _mockResultadoIndicadorRepository;
        private Mock<ILogEventoService> _mockLogEventoService;

        private Mock<IOptions<ServiceGlobalSettings>> _mockSettings;
        private SupervisaoTempoRealService _service;

        [SetUp]
        public void SetUp()
        {
            _mockResultadoIndicadorRepository = new Mock<IResultadoIndicadorRepository>();
            _mockLogEventoService = new Mock<ILogEventoService>();

            _service = new SupervisaoTempoRealService(
                _mockResultadoIndicadorRepository.Object,
                (IOptions<ServiceGlobalSettings>)_mockSettings, 
                _mockLogEventoService.Object
            );
        }

        [Test]
        public async Task BuscarAsync_Test()
        {
            var cancellationToken = new CancellationToken();

            var filter = new SupervisaoTempoRealFiltroViewModel
            {
                CodAgente = "_20ee8f9a-95dd-4923-be9e-7230b7b37233",
                CodScada = "CIA_CG1_CD_CMG_UTR",
                DataProcessar = "2023-06"
            };

            var expectedResultList = new List<SupervisaoTempoRealView>()
            {
                new SupervisaoTempoRealView
                {
                    EnderecoProtocolo = "MGTMAR2_____CH4P3_______MG____AQ",
                    CodGrandeza = "MGTMSE_289_CH4P3_S"
                },
                new SupervisaoTempoRealView
                {
                    EnderecoProtocolo = "MGPIME_138_TR5_S_KV",
                    CodGrandeza = "MGPIME_138_TR5_S_KV"
                }
            };

            _mockResultadoIndicadorRepository.Setup(x => x.SupervisaoTempoResultadoIndicador(filter.CodAgente, filter.DataProcessar, cancellationToken)).ReturnsAsync(expectedResultList);

            var result = _service.BuscarAsync(filter, cancellationToken);

            Assert.NotNull(result);
        }

        [Test]
        public async Task ListaScadaAsync_Test()
        {
            var cancellationToken = new CancellationToken();

            var filter = new SupervisaoTempoRealFiltroViewModel
            {
                CodAgente = "_20ee8f9a-95dd-4923-be9e-7230b7b37233",
                CodScada = "CIA_CG1_CD_CMG_UTR",
                DataProcessar = "2023-06"
            };

            var expectedReturnList = new List<ScadaView>()
            {
                new ScadaView
                {
                    CodLscinf = "MGCAG       ",
                    IdoOns = "CIA_CG1_UT_MGCAG_UTR"
                },
                new ScadaView
                {
                    CodLscinf = "STINACIO    ",
                    IdoOns = "CIA_CG1_UT_STINACIO_UTR"
                }
            };

            _mockResultadoIndicadorRepository.Setup(x => x.FiltrarAgentesScadaAsync(filter.CodAgente, filter.CodAgente, cancellationToken)).ReturnsAsync(expectedReturnList);

            var result = _service.ListaScadaAsync(filter, cancellationToken);

            Assert.NotNull(result);
        }
    }
}
