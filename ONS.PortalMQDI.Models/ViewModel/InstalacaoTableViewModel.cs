using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class InstalacaoTableViewModel
    {
        public string Instalacao { get; set; }
        public string Indicador { get; set; }
        public string Cos { get; set; }
        public Dictionary<string, InstalacaoItemViewModel> Meses { get; set; }
    }

    public class InstalacaoItemViewModel
    {
        public string Valor { get; set; }
        public bool Violacao { get; set; }
    }
}
