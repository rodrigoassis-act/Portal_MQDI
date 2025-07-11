using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Shared.Settings;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class SharepointService
    {
        private readonly IOptions<SharepointSettings> _settings;

        public SharepointService(IOptions<SharepointSettings> settings)
        {
            _settings = settings;
        }

        private HttpWebRequest ObterRequest(string destination, string method)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination);
            SetCredentialsInRequest(request);
            request.ContentType = "application/json;odata=verbose";
            request.Accept = "application/json;odata=verbose";
            request.Timeout = 600000;
            request.Method = method;
            return request;
        }

        private void SetCredentialsInRequest(HttpWebRequest request)
        {
            request.PreAuthenticate = true;
            NetworkCredential cred = new NetworkCredential(_settings.Value.Username, _settings.Value.Password);
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(request.Address.ToString()), "NTLM", cred);
            request.Credentials = credentialCache;
        }

        public byte[] DownloadFile(string folderName, string fileName)
        {
            byte[] fileData = null;
            string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{_settings.Value.Destination}/{folderName}/{fileName}";

            HttpWebRequest request = ObterRequest(fileUrl, "GET");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            responseStream.CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                        }
                    }
                }
            }

            return fileData;
        }

        public async Task<byte[]> DownloadFileAsync(string folderName, string fileName, CancellationToken cancellationToken)
        {
            byte[] fileData = null;
            string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}/{fileName}";

            HttpWebRequest request = ObterRequest(fileUrl, "GET");

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await responseStream.CopyToAsync(memoryStream, 81920, cancellationToken).ConfigureAwait(false);
                            fileData = memoryStream.ToArray();
                        }
                    }
                }
            }

            return fileData;
        }

        public async Task<bool> UploadFileAsync(string folderName, string fileName, Stream fileStream, CancellationToken cancellationToken)
        {
            try
            {
                string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{_settings.Value.Destination}/{folderName}/{fileName}";
                HttpWebRequest request = ObterRequest(fileUrl, "PUT");

                using (Stream requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await fileStream.CopyToAsync(requestStream, 81920, cancellationToken).ConfigureAwait(false);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {
                    return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFile(string folderName, string fileName)
        {
            try
            {
                string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}/{fileName}";
                HttpWebRequest request = ObterRequest(fileUrl, "DELETE");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }


        public bool UploadFile(string folderName, string fileName, byte[] fileData)
        {
            try
            {
                if (FileExists(folderName, fileName))
                {
                    DeleteFile(folderName, fileName);
                }

                string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}/{fileName}";
                HttpWebRequest request = ObterRequest(fileUrl, "PUT");

                using (MemoryStream memoryStream = new MemoryStream(fileData))
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        memoryStream.CopyTo(requestStream);
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool FileExists(string folderName, string fileName)
        {
            try
            {
                string fileUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}/{fileName}";
                HttpWebRequest request = ObterRequest(fileUrl, "HEAD");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }


        public string CreateFolder(string folderName)
        {
            try
            {
                string folderUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}";
                HttpWebRequest request = ObterRequest(folderUrl, "HEAD");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return folderName;
                    }
                }
            }
            catch (WebException e) when ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                string folderUrl = $"{_settings.Value.URL}/{_settings.Value.Destination}/{_settings.Value.SubFolder}/{folderName}";
                HttpWebRequest request = ObterRequest(folderUrl, "MKCOL");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.Created ? folderName : null;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
