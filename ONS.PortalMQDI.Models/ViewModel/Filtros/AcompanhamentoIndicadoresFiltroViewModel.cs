using ONS.PortalMQDI.Models.Model;

namespace ONS.PortalMQDI.Models.ViewModel.Filtros
{
    public class AcompanhamentoIndicadoresFiltroViewModel : BasePaginator
    {
        public string Agente { get; set; }
        public bool? FlgViolacaoAnual { get; set; }
        public string MesAno { get; set; }
        public string TpIndicador { get; set; }
    }
}
