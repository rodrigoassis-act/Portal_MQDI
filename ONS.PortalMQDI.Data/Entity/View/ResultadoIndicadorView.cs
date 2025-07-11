using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ResultadoIndicadorView
    {
        [Column("UTR_CD")]
        [Key]
        public string UtrCd { get; set; }

        [Column("cod_lscinf")]
        public string CodLscinf { get; set; }

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

        [Column("cod_tpindicador")]
        public string CodIndicador { get; set; }
    }
}
