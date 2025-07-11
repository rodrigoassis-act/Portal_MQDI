using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class ResultadoTableViewModel
    {
        public string SSCLCD { get; set; }
        public string Indicador { get; set; }
        public string Cos { get; set; }
        public Dictionary<string, ResultadoItemViewModel> Meses { get; set; }
    }
    public class ResultadoItemViewModel
    {
        public string Valor { get; set; }
        public bool Violacao { get; set; }
    }
}
