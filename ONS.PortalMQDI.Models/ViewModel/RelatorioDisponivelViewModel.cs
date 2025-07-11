using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class RelatorioDisponivelViewModel
    {
        public List<SelectItemViewModel<string>> Data { get; set; }
        public List<SelectItemViewModel<string>> Agente { get; set; }
    }
}
