
using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class AgenteTableViewModel
    {
        public string Agente { get; set; }
        public string Indicador { get; set; }
        public Dictionary<string, AgenteItemViewModel> Meses { get; set; }
    }

    public class AgenteItemViewModel
    {
        public string Valor { get; set; }
        public bool Violacao { get; set; }
    }
}
