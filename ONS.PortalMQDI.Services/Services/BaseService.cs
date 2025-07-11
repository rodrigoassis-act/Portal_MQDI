using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public abstract class BaseService
    {

        private ILog _logger = null;


        protected virtual ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(GetType());
                }
                return _logger;
            }
        }

        protected virtual ICredentials GetSharepointCredentials()
        {
            return new NetworkCredential("", "");
        }


        protected virtual string GetSharepointUrl()
        {
            return "" + "";
        }

        protected virtual HttpClient client
        {
            get
            {
                throw new NotImplementedException("client não implementando, faça o override em seu service");
            }
        }

        public HttpResponseMessage Http(string url, HttpRequestMessage request, HttpClient cliente = null)
        {
            HttpClient c = cliente == null ? client : cliente;

            var Request = CloneHttpRequestMessageAsync(request).Result;
            Request.RequestUri = new Uri(c.BaseAddress + url);
            Logger.Debug($"Http.URI = {Request.RequestUri.ToString()}");

            HttpResponseMessage response = c.SendAsync(Request).Result;
            Logger.Debug($"Http.Response.Success = {response.IsSuccessStatusCode}");

            return response;
        }

        public async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            var ms = new MemoryStream();
            if (req.Method != HttpMethod.Get && req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }

            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                if (header.Key != "Origin")
                {
                    clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }


        protected virtual string CombineUrl(string baseUri, string relativeUri)
        {
            return new Uri(new Uri(baseUri.EndsWith("/") ? baseUri : $"{baseUri}/"),
                           (relativeUri.StartsWith("/") ? relativeUri.Substring(1) : relativeUri)).AbsoluteUri;
        }



    }
}
