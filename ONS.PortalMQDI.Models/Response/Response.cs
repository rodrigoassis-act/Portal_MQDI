
using System.Net;


namespace ONS.PortalMQDI.Models.Response
{
    public class Response
    {
        public Response(HttpStatusCode codigo, string error, string nomeEndpoint)
        {
            this.Codigo = codigo;
            if (codigo == HttpStatusCode.OK)
            {
                this.Mensagem = "Solicitação executada com sucesso.";
            }
            else if (codigo == HttpStatusCode.Created)
            {
                this.Mensagem = "Recurso criado com sucesso.";
            }
            else if (codigo == HttpStatusCode.BadRequest)
            {
                this.Mensagem = error;
            }
            else if (codigo == HttpStatusCode.Unauthorized)
            {
                this.Mensagem = "Sem autorização.";
            }
            else if (codigo == HttpStatusCode.Forbidden)
            {
                this.Mensagem = $"Sem permissão no endpoint: {nomeEndpoint}.";
            }
            else if (codigo == HttpStatusCode.NotFound)
            {
                this.Mensagem = string.IsNullOrEmpty(error) ? "Endpoint não encontrado. Verifique se a URL está correta e tente novamente." : error;
            }
            else if (codigo == HttpStatusCode.InternalServerError)
            {
                this.Mensagem = error;
            }
            else
            {
                this.Mensagem = "Erro desconhecido.";
            }
        }

        public HttpStatusCode Codigo { get; set; }
        public string Mensagem { get; set; }
    }
}
