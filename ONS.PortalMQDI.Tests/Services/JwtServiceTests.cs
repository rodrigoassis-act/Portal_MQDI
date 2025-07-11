//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using Moq;
//using NUnit.Framework;
//using ONS.PortalMQDI.Services.Services;
//using ONS.PortalMQDI.Shared.Settings;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ONS.PortalMQDI.Tests.Services
//{
//    public class JwtServiceTests
//    {
//        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
//        private JwtService _jwtService;
//        private PopService _popService;

//        [SetUp]
//        public void SetUp()
//        {
//            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

//            var settings = Options.Create(new PopServiceSettings
//            {
//                Uri = "https://poptst.ons.org.br/ons.pop.federation/oauth2/token",
//                Origin = "https://tst-git-087.ons.org.br",
//                ClientId = "PortalMQDI",
//                GrantType = "password",
//                Username = "ONS\\jeive.spassu",
//                Password = "Suporte@2023"
//            });

//            _popService = new PopService(settings);
//            _jwtService = new JwtService(_mockHttpContextAccessor.Object);
//        }

//        [Test]
//        public async Task TestGetClaimFromJwt_WithRealToken_ReturnsClaims()
//        {
//            // Arrange
//            var token = await _popService.GetTokenAsync();
//            var context = new DefaultHttpContext();
//            context.Request.Headers["Authorization"] = $"Bearer {token}";

//            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(context);

//            // Act
//            var result = _jwtService.GetClaimFromJwt();

//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Count() > 0);

//        }
//    }
//}
