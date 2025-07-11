using Moq;
using NUnit.Framework;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Tests.Services
{
    public class ParametroSistemaServiceTests
    {
        private Mock<IValorParametroSistemaRepository> _mockValorParametroSistemaRepository;
        private Mock<IResultadoIndicadorRepository> _mockResultadoIndicadorRepository;
        private Mock<ICalendarioSistemaRepository> _mockCalendarioSistemaRepository;
        private Mock<ICalendarioService> _mockCalendarioService;
        private ParametroSistemaService _service;

        [SetUp]
        public void SetUp()
        {
            _mockValorParametroSistemaRepository = new Mock<IValorParametroSistemaRepository>();
            _mockResultadoIndicadorRepository = new Mock<IResultadoIndicadorRepository>();
            _mockCalendarioSistemaRepository = new Mock<ICalendarioSistemaRepository>();
            _mockCalendarioService = new Mock<ICalendarioService>();

            _service = new ParametroSistemaService(
                _mockValorParametroSistemaRepository.Object,
                _mockResultadoIndicadorRepository.Object,
                _mockCalendarioSistemaRepository.Object,
                _mockCalendarioService.Object
            );

            var parametros = new List<ValorParametroSistema>
            {
                new ValorParametroSistema { IdParametroSistema = 1, ParametroSistema = new Parametro { IdParametro = 1, NomeParametro = "Param 1", TipoParametro = "Tipo 1" }, ValParametro = "Valor 1" },
                new ValorParametroSistema { IdParametroSistema = 2, ParametroSistema = new Parametro { IdParametro = 2, NomeParametro = "Param 2", TipoParametro = "Tipo 2" }, ValParametro = "Valor 2" },
            };

            _mockValorParametroSistemaRepository
                .Setup(x => x.BuscarValorLimiteAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ValorParametroSistema>>(parametros));
        }

        [Test]
        public async Task BuscaValorLimiteDeIndicadoresAsync_Test()
        {
            var cancellationToken = new CancellationToken();

            var result = await _service.BuscaValorLimiteDeIndicadoresAsync(cancellationToken);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Param 1", result[0].NomParametro);
            Assert.AreEqual("Tipo 1", result[0].NomTpParametro);
            Assert.AreEqual("Valor 1", result[0].ValParametro);
            Assert.AreEqual("Param 2", result[1].NomParametro);
            Assert.AreEqual("Tipo 2", result[1].NomTpParametro);
            Assert.AreEqual("Valor 2", result[1].ValParametro);
        }
    }
}
