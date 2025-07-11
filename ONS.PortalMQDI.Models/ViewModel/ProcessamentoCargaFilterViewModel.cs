using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class ProcessamentoCargaFilterViewModel
    {
        public List<string> AnoMes { get; set; }
        public List<string> AgeMrid { get; set; }
        public List<string> Centro { get; set; }
        public string ProcRelatorioTipo { get; set; }
    }
}
