using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ConsultaIndicadorInstalacaoView
    {
        [Column("nom_tprecurso")]
        [Key]
        public string NomTpRecurso { get; set; }

        [Column("nom_curto")]
        public string NomCurto { get; set; }

        [Column("nom_longo")]
        public string NomLongo { get; set; }

        [Column("ido_ons")]
        public string IdoOns { get; set; }

        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("mrid")]
        public string Mrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("id_tprecurso")]
        public int IdTprecurso { get; set; }

        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }

        [Column("val_mensal")]
        public double ValMensal { get; set; }

        [Column("val_anual")]
        public double ValAnual { get; set; }

        [Column("flg_violacaomensal")]
        public bool FlgViolacaoMensal { get; set; }

        [Column("flg_violacaoanual")]
        public bool FlgViolacaoAnual { get; set; }

        [Column("din_recebimentoresultado")]
        public DateTime DinRecebimentoResultado { get; set; }

        [Column("id_resultadoindicadorrecalculado")]
        public int? IdResultadoIndicadorRecalculado { get; set; }

        [Column("nom_tpindicador")]
        public string NomTpIndicador { get; set; }

        [Column("cod_tpindicador")]
        public string CodTpIndicador { get; set; }

        [Column("ins_mrid")]
        public string IdInstalacao { get; set; }

        [Column("dsc_contestacao")]
        public string DscAnalistaConstestacao { get; set; }

        [Column("cmn_resultado")]
        public string DscOnsConstestacao { get; set; }
    }
}
