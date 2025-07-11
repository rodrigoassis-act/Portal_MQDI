using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class MedidaSupervisionadaRecursoViewModel
    {
        public string IdInstalacao { get; set; }
        public string NomeInstalacao { get; set; }
        public List<MedidaSupervisionadaRecursoItemViewModel> Recurso { get; set; }
    }

    public class MedidaSupervisionadaRecursoItemViewModel
    {
        public string CosId { get; set; }
        public string IdoOns { get; set; }
        public string DscGrandeza { get; set; }
        public string TpRede { get; set; }
    }
}
