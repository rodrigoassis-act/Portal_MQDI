using Microsoft.SharePoint.Client;
using System.Net;

namespace ONS.PortalMQDI.Shared.Utils
{
    public class SharepointUtil
    {
        private string _siteUrl;
        private string _username;
        private string _password;
        private ClientContext _context;

        public SharepointUtil()
        {
            InitializeContext();
        }

        private void InitializeContext()
        {
            _context = new ClientContext(_siteUrl);
            _context.Credentials = new NetworkCredential("", "");
        }

        private System.Security.SecureString ConvertToSecureString(string input)
        {
            var secureString = new System.Security.SecureString();
            foreach (char c in input)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        public void UploadDocument(string libraryPath, string localFilePath, string destFileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(localFilePath, System.IO.FileMode.Open))
            {
                FileCreationInformation fileCreationInfo = new FileCreationInformation();
                fileCreationInfo.ContentStream = fs;
                fileCreationInfo.Url = libraryPath + destFileName;
                fileCreationInfo.Overwrite = true;

                Web web = _context.Web;
                List docs = web.GetList(libraryPath);
                Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(fileCreationInfo);

                _context.Load(uploadFile);
                _context.ExecuteQuery();
            }
        }

    }
}
