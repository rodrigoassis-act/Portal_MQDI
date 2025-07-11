namespace ONS.PortalMQDI.Models.Response
{
    public class AngularSettingsResponse
    {
        public string Menu { get; set; }
        public string MenuCDN { get; set; }
        public string Version { get; set; }

        public PermisaoResponse Permisao { get; set; }
        public PopServiceResponse PopService { get; set; }
        public ConfigResponse Config { get; set; }
        public MensagemAvisoResponse MensagemAviso { get; set; }

        public AngularSettingsResponse()
        {
        }
    }

    public class PermisaoResponse
    {
        public bool IsAdministratorOns { get; set; }
        public bool IsAgente { get; set; }
    }

    public class PopServiceResponse
    {
        public string Uri { get; set; }
        public string Origin { get; set; }
        public string ClientId { get; set; }
        public string GrantType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class ConfigResponse
    {
        public string Version { get; set; }
    }

    public class MensagemAvisoResponse
    {
        public string Mensagem { get; set; }
    }
}
