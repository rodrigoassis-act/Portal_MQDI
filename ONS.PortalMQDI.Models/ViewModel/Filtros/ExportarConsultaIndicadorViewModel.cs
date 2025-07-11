using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONS.PortalMQDI.Models.ViewModel.Filtros
{
    public class ExportarConsultaIndicadorViewModel
    {
        [Required]
        public List<string> Agentes { get; set; }
        [Required]
        public string MesAno { get; set; }

        public string Indicador { get; set; }

        public bool? Violacao { get; set; }
    }
}
