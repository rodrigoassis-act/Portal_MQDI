using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_resultadodiario")]
    public class ResultadoDiario
    {
        [Key]
        [Column("id_resultadodiario")]
        public int IdResultadoDiario { get; set; }

        [Required]
        [Column("dat_resultado")]
        public DateTime DataResultado { get; set; }

        [Required]
        [Column("cos_id")]
        public string CosId { get; set; }

        [Required]
        [Column("mrid")]
        public string AgeMrid { get; set; }

        [Required]
        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Required]
        [Column("id_tprecurso")]
        public int IdTpRecurso { get; set; }

        [Required]
        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }

        [Required]
        [Column("val_dispdiario")]
        public float ValDispDiario { get; set; }

        [Column("flg_dispdiario")]
        public bool FlagDispDiario { get; set; }

        [ForeignKey("IdTpIndicador")]
        public virtual TpIndicador? TpIndicador { get; set; }
    }
}
