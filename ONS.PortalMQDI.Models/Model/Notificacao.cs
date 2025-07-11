using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.Model
{
    public class Notificacao
    {
        public string Origem { get; set; } = "PortalMQDI";
        public string Titulo { get; set; }
        public CorpoNotificacao Corpo { get; set; }
        public CorpoEmail CorpoEmail { get; set; }
        public CorpoMobile CorpoMobile { get; set; }
        public Acao acoes { get; set; }
        public Audiencia Audiencia { get; set; }
        public List<Claims> Claims { get; set; }
        public object Payload { get; set; }
        public bool? CriacaoAutomatica { get; set; }
        public Dictionary<string, string> Parametros { get; set; }
    }

    public class CorpoNotificacao
    {
        public string Mensagem { get; set; }
        public string Formato { get; set; }
    }
    public class CorpoEmail
    {
        public string MensagemEmail { get; set; }
        public string Formato { get; set; }
    }
    public class CorpoMobile
    {
        public string MensagemMobile { get; set; }
    }
    public class Acao
    {
        public Http Http { get; set; }
    }

    public class Http
    {
        public string Nome { get; set; }
        public string Url { get; set; }
    }

    public class Audiencia
    {
        public List<Topico> Topicos { get; set; }
    }

    public class Topico
    {
        public string Nome { get; set; }
    }

    public class Claims
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public TipoPermissao RoleType { get; set; }
    }

    public enum TipoPermissao
    {
        Leitura = 0,
        Colaboracao,
        Administracao
    }
}
