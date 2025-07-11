namespace ONS.PortalMQDI.Models.ViewModel
{
    public class ContestacaoAnalistaViewModel
    {
        public int? IdConstestacao { get; set; }
        public int IdResultadoIndicador { get; set; }
        public string ComentarioAnalista { get; set; }
        public string ComentarioOns { get; set; }
        public bool? TipoStatus { get; set; }

    }
}
