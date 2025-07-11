using ONS.PortalMQDI.Models.Model;
using System;

namespace ONS.PortalMQDI.Models.ViewModel.Filtros
{
    public class AgenteFiltroViewModel : FiltroBase
    {
        public string MesAno { get; set; }

        public string MesAnoFormatada()
        {
            if (!String.IsNullOrEmpty(MesAno))
            {
                return this.MesAno.Replace('/', '-');
            }
            return null;
        }
    }
}
