using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ResultadoDiarioDCDView
    {
        [Column("id_resultadodiario")]
        [Key]
        public int IdResultadoDiario { get; set; }

        [Column("UTR_CD")]
        public string UtrCd { get; set; }

        [Column("cod_lscinf")]
        public string CodLscinf { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("mrid")]
        public string Mrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("id_tprecurso")]
        public int IdTpRecurso { get; set; }

        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }

        [Column("dat_resultado")]
        public DateTime DataResultado { get; set; }

        [Column("val_dispdiario")]
        public double ValDispDiario { get; set; }

        [Column("flg_dispdiario")]
        public int FlgDispDiario { get; set; }

        [Column("cod_tpindicador")]
        public string CodTpIndicador { get; set; }

        [Column("dono")]
        public string Dono { get; set; }
    }
}
