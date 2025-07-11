using ONS.PortalMQDI.Models.ViewModel;
using System;
using System.Collections.Generic;


namespace ONS.PortalMQDI.Models.Model
{
    public class AutomacaoIndicador
    {
        public ArquivoViewModel Arquivo { get; set; }
        public string AnoMes { get; set; }
        public string NomeAgente { get; set; }
        public string IdAgente { get; set; }
        public int TipoRelatorio { get; set; }

        public List<MedidaSupervisionadaRecursoViewModel> EntryMedidas { get; set; }
        public List<object> EntryAgentes { get; set; }

        public List<object> EntryResultadoIndicador { get; set; }

        public List<object> EntryInstalacao { get; set; }

        public List<object> EntryIndicadores { get; set; }
    }
}
