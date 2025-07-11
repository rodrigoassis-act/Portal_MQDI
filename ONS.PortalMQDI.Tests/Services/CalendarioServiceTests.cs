using Moq;
using NUnit.Framework;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Tests.Services
{
    public class CalendarioServiceTests
    {
        private Mock<ICalendarioSistemaRepository> _mockCalendarioSistemaRepository;
        private Mock<IValorParametroSistemaRepository> _mockValorParametroSistemaRepository;
        private Mock<IFeriadoRepository> _mockFeriadoRepository;
        private Mock<IRelatorioRepository> _relatorioRepository;
        private Mock<ICalendarioCalculator> _mockCalendarioCalculator;
        private Mock<IServiceProvider> _serviceProvider;
        private Mock<ILogEventoService> _mockLogEventoService;
        private Mock<ICargaService> _mockCargaService;

        private CalendarioService _service;

        [SetUp]
        public void SetUp()
        {
            _mockCalendarioSistemaRepository = new Mock<ICalendarioSistemaRepository>();
            _mockValorParametroSistemaRepository = new Mock<IValorParametroSistemaRepository>();
            _mockFeriadoRepository = new Mock<IFeriadoRepository>();
            _relatorioRepository = new Mock<IRelatorioRepository>();
            _mockCalendarioCalculator = new Mock<ICalendarioCalculator>();
            _serviceProvider = new Mock<IServiceProvider>();
            _mockLogEventoService = new Mock<ILogEventoService>();
            _mockCargaService = new Mock<ICargaService>();

            _service = new CalendarioService(_mockCalendarioSistemaRepository.Object, _mockValorParametroSistemaRepository.Object, _mockFeriadoRepository.Object, _relatorioRepository.Object, _mockCalendarioCalculator.Object, _serviceProvider.Object, _mockLogEventoService.Object, _mockCargaService.Object);
        }

        [Test]
        public async Task RetornaDatasDoSistemaAsync_Test()
        {
            var filtro = new AgenteFiltroViewModel { MesAno = "01/2023" };
            var calendario = new CalendarioSistema
            {
                DataLiberacaoContestacoesAgente = DateTime.Now,
                DataReferenciaIndicador = "02/2012",
                DataTerminoPeriodoAnaliseContestacoes = DateTime.Now,
                DataTerminoPeriodoContestacoesAgente = DateTime.Now,
                IdCalendarioSistema = 1
            };

            var parametro = new ValorParametroSistema
            {
                DataFimVigencia = DateTime.Now,
                IdParametro = 1,
                IdParametroSistema = 1,
                ParametroSistema = new Parametro
                {
                    IdParametro = 1,
                    NomeParametro = "test",
                    TipoParametro = "test",
                },
                ValParametro = "1"

            };

            _mockCalendarioSistemaRepository.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CalendarioSistema, bool>>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new List<CalendarioSistema> { calendario }.AsEnumerable()));
            _mockValorParametroSistemaRepository.Setup(x => x.BuscarParametroTerminoPeriodoAnaliseContestacaoAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(parametro));

            var result = await _service.RetornaDatasDoSistemaAsync(filtro, default);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DatasDisponiveisParaSelecaoAsync_Test()
        {
            var calendarios = new List<CalendarioSistema>
            {
                new CalendarioSistema { DataReferenciaIndicador = "2023-07-30" },
                new CalendarioSistema { DataReferenciaIndicador = "2023-07-29" },
                new CalendarioSistema { DataReferenciaIndicador = "2023-07-28" },
                new CalendarioSistema { DataReferenciaIndicador = "2023-07-27" }
            };

            _mockCalendarioSistemaRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(calendarios.AsEnumerable()));

            var result = await _service.DatasDisponiveisParaSelecaoAsync(default);

            Assert.NotNull(result);
            Assert.True(result.Count() == calendarios.Count());
        }
    }
}
