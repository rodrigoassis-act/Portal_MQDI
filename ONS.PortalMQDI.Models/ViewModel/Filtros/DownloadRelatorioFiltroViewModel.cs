namespace ONS.PortalMQDI.Models.ViewModel.Filtros
{
    public class DownloadRelatorioFiltroViewModel
    {
        private string mesAnoSelecionado { get; set; }
        public string MesAnoSelecionado
        {
            get
            {
                if (!string.IsNullOrEmpty(mesAnoSelecionado))
                {
                    return mesAnoSelecionado.Replace("/", "-");
                }
                else
                {
                    return mesAnoSelecionado;
                }
            }
            set { mesAnoSelecionado = value; }
        }
        public string AgenteSelecionado { get; set; }
        public int? TpTipoRelatorio { get; set; }
        public string TpIndicadorSelecionado { get; set; }
        public string Indicador { get; set; }
        public string IdOnsAgente { get; set; }
        public string Relatorio { get; set; }
    }
}
