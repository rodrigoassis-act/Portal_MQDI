using Moq;
using NUnit.Framework;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Tests.Services
{
    public class AgenteServiceTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IAgenteRepository> _mockAgenteRepository;
        private Mock<ITpRelatorioRepository> _mockTpRelatorioRepository;
        private Mock<IRelatorioRepository> _mockRelatorioRepository;

        private AgenteService _service;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _mockAgenteRepository = new Mock<IAgenteRepository>();
            _mockTpRelatorioRepository = new Mock<ITpRelatorioRepository>();
            _mockRelatorioRepository = new Mock<IRelatorioRepository>();
            _service = new AgenteService(
                _mockUserService.Object,
                _mockAgenteRepository.Object,
                _mockTpRelatorioRepository.Object,
                _mockRelatorioRepository.Object
            );

            var cancellationToken = new CancellationToken();

            var agentes = new List<Agente>
            {
                new Agente { AnoMesReferencia = "202307", NomeCurto = "Agente 1", AgeMrid = "1", NomeLongo = "Agente Longo 1" },
                new Agente { AnoMesReferencia = "202307", NomeCurto = "Agente 2", AgeMrid = "2", NomeLongo = "Agente Longo 2" },
                new Agente { AnoMesReferencia = "202307", NomeCurto = "Agente 3", AgeMrid = "3", NomeLongo = "Agente Longo 3" },
                new Agente { AnoMesReferencia = "202307", NomeCurto = "Agente 4", AgeMrid = "4", NomeLongo = "Agente Longo 4" }
            };

            _mockAgenteRepository.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Agente, bool>>>(), cancellationToken)).Returns(Task.FromResult(agentes.AsEnumerable()));

        }

        [Test]
        public async Task TestBuscarAgenteAsync_UserIsAdmin_ReturnsExpectedViewModel()
        {
            var filtroViewModel = new AgenteFiltroViewModel()
            {
                MesAno = "2023/06"
            };

            var expectedReturnList = new List<Agente>()
            {
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 1", AgeMrid = "1", NomeLongo = "Agente Longo 1" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 2", AgeMrid = "2", NomeLongo = "Agente Longo 2" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 3", AgeMrid = "3", NomeLongo = "Agente Longo 3" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 4", AgeMrid = "4", NomeLongo = "Agente Longo 4" }
            };

            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(x => x.IsAdministrator()).Returns(true);
            _mockAgenteRepository.Setup(x => x.TodosAgentePorGrandezaAsync("2023-06", cancellationToken, null)).ReturnsAsync(expectedReturnList);

            var result = await _service.BuscarAgenteAsync(filtroViewModel, cancellationToken);

            Assert.NotNull(result);
            Assert.True(result.Count() == 4);
        }

        [Test]
        public async Task TestBuscarAgenteAsync_UserIsNotAdmin_ReturnsExpectedViewModel()
        {
            var filtroViewModel = new AgenteFiltroViewModel()
            {
                MesAno = "2023/06"
            };

            var expectedReturnList = new List<Agente>()
            {
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 1", AgeMrid = "1", NomeLongo = "Agente Longo 1" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 2", AgeMrid = "2", NomeLongo = "Agente Longo 2" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 3", AgeMrid = "3", NomeLongo = "Agente Longo 3" },
                new Agente { AnoMesReferencia = "2023-07", NomeCurto = "Agente 4", AgeMrid = "4", NomeLongo = "Agente Longo 4" }
            };

            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(x => x.IsAdministrator()).Returns(false);
            _mockUserService.Setup(x => x.ListaEscopos()).Returns(new List<string>() { "Escopo1", "Escopo2" });
            _mockAgenteRepository.Setup(x => x.TodosAgentePorGrandezaAsync("2023-06", cancellationToken, new List<string>() { "Escopo1", "Escopo2" })).ReturnsAsync(expectedReturnList);

            var result = await _service.BuscarAgenteAsync(filtroViewModel, cancellationToken);

            Assert.NotNull(result);
            Assert.True(result.Count() == 4);
        }
    }
}
