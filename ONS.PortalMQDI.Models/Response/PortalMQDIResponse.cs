using System.Net;


namespace ONS.PortalMQDI.Models.Response
{
    public class PortalMQDIResponse : Response
    {
        public PortalMQDIResponse(HttpStatusCode codigo, object _retorno = null, string error = null, string nomeEndpoint = null) : base(codigo, error, nomeEndpoint)
        {
            this.Retorno = _retorno;
        }

        public object Retorno { get; set; }
    }
}
