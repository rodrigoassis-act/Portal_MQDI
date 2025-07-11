using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Settings;
using System;
using System.Net.Http;
using System.Text;

namespace ONS.PortalMQDI.Services.Services
{
    public class NotificacaoService : BaseService
    {
        private readonly IOptions<ServiceGlobalSettings> _settings;

        private readonly IUserService _userService;
        public NotificacaoService(IUserService userService, IOptions<ServiceGlobalSettings> settings)
        {
            _userService = userService;
            _settings = settings;
        }

        protected override HttpClient client
        {
            get
            {
                return new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(1),
                    BaseAddress = new Uri(_settings.Value.NotificacaoApi)
                };
            }
        }

        public void Send(Notificacao notificacao)
        {

            try
            {
                HttpRequestMessage requestRequest = new HttpRequestMessage();
                requestRequest.Method = HttpMethod.Post;
                requestRequest.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(notificacao), Encoding.UTF8, "application/json");
                var temp = Newtonsoft.Json.JsonConvert.SerializeObject(notificacao);
                requestRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userService.Authorization());
                var response = Http("Notificacao", requestRequest);

                if (response.IsSuccessStatusCode == false)
                {
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
