using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class IndicadorViewModel
    {
        public List<AgenteViewModel> Agente { get; set; }
        public List<string> DynamicHeader { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
    }
}
