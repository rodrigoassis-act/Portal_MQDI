using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ONS.PortalMQDI.Shared.Utils
{
    public class HTTPUtil
    {
        public T Post<T>(string url, object body, Dictionary<string, string> headers)
        {
            ILog log = LogManager.GetLogger(typeof(HTTPUtil));
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json"),
            };
            request.Headers.Clear();
            var e = headers.GetEnumerator();
            while (e.MoveNext())
            {
                request.Headers.Add(e.Current.Key, e.Current.Value);
            }
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,de-DE;q=0.8,de;q=0.7,en-US;q=0.6,en;q=0.5");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");

            var response = httpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            log.Info(content);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public T Get<T>(string url)
        {
            ILog log = LogManager.GetLogger(typeof(HTTPUtil));
            var httpClient = new HttpClient();
            //httpClient.Timeout = TimeSpan.FromMilliseconds(1000);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            var response = httpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            log.Debug(content);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public bool Head(string url)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Head
            };

            var response = httpClient.SendAsync(request).Result;
            return response.StatusCode == System.Net.HttpStatusCode.OK;

        }
    }
}
