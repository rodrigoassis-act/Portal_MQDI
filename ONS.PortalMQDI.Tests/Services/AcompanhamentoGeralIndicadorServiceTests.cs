using Moq;
using NUnit.Framework;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Tests.Services
{
    public class AcompanhamentoGeralIndicadorServiceTests
    {
        private Mock<IAgenteRepository> _mockAgenteRepository;
        private Mock<IResultadoIndicadorRepository> _mockResultadoIndicadorRepository;
        private Mock<IInstalacaoRepository> _mockInstalacaoRepository;
        private AcompanhamentoGeralIndicadorService _service;
        private Mock<ILogEventoService> _mockLogEventoService;

        [SetUp]
        public void SetUp()
        {
            _mockAgenteRepository = new Mock<IAgenteRepository>();
            _mockResultadoIndicadorRepository = new Mock<IResultadoIndicadorRepository>();
            _mockInstalacaoRepository = new Mock<IInstalacaoRepository>();
            _mockLogEventoService = new Mock<ILogEventoService>();

            _service = new AcompanhamentoGeralIndicadorService(
                _mockAgenteRepository.Object,
                _mockResultadoIndicadorRepository.Object,
                _mockInstalacaoRepository.Object,
                _mockLogEventoService.Object
            );
        }

        [Test]
        public async Task TestFiltroAsync_ValidInput_ReturnsExpectedViewModel()
        {
            // Arrange
            var filtroViewModel = new AcompanhamentoIndicadoresFiltroViewModel
            {
                Agente = "test",
                TpIndicador = "test",
                FlgViolacaoAnual = true,
                MesAno = "02/2012",
            };

            var cancellationToken = new CancellationToken();

            var expectedAgenteList = new List<AgenteIndicadorView>()
            {
                new AgenteIndicadorView
                {
                    AgeMrid = "test",
                    NomeIndicador = "test",
                    CosId = "1",
                    AnoMesReferencia = "test",
                    CodIndicador = "teste",
                    DinRecebimentoResultado = DateTime.Now,
                    FlgViolacaoAnual = true,
                    FlgViolacaoMensal = true,
                    IdOns = "",
                    IdResultadoIndicador = 1,
                    IdResultadoIndicadorRecalculado = 1,
                    IdTipoRecurso = 1,
                    IdTpIndicador  = 1,
                    Mrid = "1",
                    NomeCurto = "1",
                    NomeLongo = "1",
                    NomeTipoRecurso = "1",
                    ValAnual = 1,
                    ValMensal = 1
                }
            };
            var expectedResultadoList = new List<ResultadoIndicadorView>() {

                new ResultadoIndicadorView
                {
                    AgeMrid = "test",
                    CosId = "test",
                    ValMensal = 14,
                    ValAnual = 20,
                    AnoMesReferencia = "02/2012",
                    Mrid = "test",
                    IdTpIndicador = 12,
                    CodIndicador = "test",
                    CodLscinf = "test",
                    DinRecebimentoResultado = DateTime.Now,
                    FlgViolacaoAnual = false,
                    FlgViolacaoMensal = true,
                    IdResultadoIndicador = 1,
                    IdResultadoIndicadorRecalculado = 1,
                    IdTprecurso = 1,
                    UtrCd = "test"
                }
            };
            var expectedInstalacaoList = new List<InstalacaoView>()
            {
                new InstalacaoView
                {
                    AgeMrid = "test",
                    AnoMesReferencia = "02/2012",
                    CodTpIndicador = "test",
                    ValMensal = 14,
                    ValAnual = 20,
                    CosId = "test",
                    IdTprecurso = 1,
                    IdResultadoIndicadorRecalculado= 12,
                    DinRecebimentoResultado= DateTime.Now,
                    FlgViolacaoAnual= true,
                    FlgViolacaoMensal= true,
                    IdoOns = "test",
                    IdResultadoIndicador = 1,
                    IdTpIndicador = 1,
                    Mrid = "test",
                    NomCurto = "test",
                    NomLongo = "test",
                    NomTpIndicador= "test",
                    NomTpRecurso = "test",
                }
            };

            _mockAgenteRepository.Setup(x => x.BuscarViewPorFiltroAsync(
                It.IsAny<List<DateTime>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expectedAgenteList);

            _mockResultadoIndicadorRepository.Setup(x => x.BuscarViewPorFiltroAsync(
                It.IsAny<List<DateTime>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expectedResultadoList);

            _mockInstalacaoRepository.Setup(x => x.BuscarViewPorFiltroAsync(
                It.IsAny<List<DateTime>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(expectedInstalacaoList);

            // Act
            var result = await _service.FiltroAsync(filtroViewModel, cancellationToken);

            // Assert
            Assert.IsNotNull(result);

        }

    }
}
