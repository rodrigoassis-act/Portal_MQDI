using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Shared.Settings;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class PopService
    {
        private readonly IOptions<PopServiceSettings> _settings;

        public PopService(IOptions<PopServiceSettings> settings)
        {
            _settings = settings;
        }

        public async Task<string> GetTokenAsync()
        {
            var client = new HttpClient();
            var uri = _settings.Value.Uri;

            client.DefaultRequestHeaders.Add("Origin", _settings.Value.Origin);

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _settings.Value.ClientId),
                new KeyValuePair<string, string>("grant_type", _settings.Value.GrantType),
                new KeyValuePair<string, string>("username", _settings.Value.Username),
                new KeyValuePair<string, string>("password", _settings.Value.Password)
            };

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                return data.access_token;
            }

            return string.Empty;
        }
    }
}
