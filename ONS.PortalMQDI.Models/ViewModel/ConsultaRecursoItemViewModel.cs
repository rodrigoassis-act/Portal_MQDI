using System;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class ConsultaRecursoItemViewModel
    {
        public string IdInstalacao { get; set; }
        public int IdResultadoIndicador { get; set; }

        public string CosId { get; set; }

        public string IdoOns { get; set; }

        public string DscGrandeza { get; set; }

        public string NomEnderecoFisico { get; set; }

        public string Mrid { get; set; }

        public string AnoMesReferencia { get; set; }

        public string AgeMrid { get; set; }

        public int IdTprecurso { get; set; }

        public int IdTpIndicador { get; set; }

        public double ValMensal { get; set; }

        public double ValAnual { get; set; }

        public bool FlgViolacaoMensal { get; set; }

        public bool FlgViolacaoAnual { get; set; }

        public DateTime DinRecebimentoResultado { get; set; }

        public int? IdResultadoIndicadorRecalculado { get; set; }

        public string TpRede { get; set; }

        public string CodLscinf { get; set; }

        public string DscAnalistaConstestacao { get; set; }

        public string DscOnsConstestacao { get; set; }

        public bool ConstestacaoStatus { get; set; }

        public int? IdConstestacao { get; set; }

        public string NomeInstalacao { get; set; }
        public bool Sgi { get; set; }
    }
}
