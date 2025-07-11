using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class AcompanhamentoIndicadoresViewModel
    {
        public List<string> DisplayColumns { get; set; }
        public List<AgenteTableViewModel> Agente { get; set; }
        public List<ResultadoTableViewModel> ResultadoIndicador { get; set; }
        public List<InstalacaoTableViewModel> Instalacao { get; set; }
    }
}
