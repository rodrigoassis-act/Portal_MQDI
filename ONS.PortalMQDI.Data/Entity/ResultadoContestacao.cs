using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_resultadocontestacao")]
    public class ResultadoContestacao
    {
        [Key]
        [Column("id_resultadocontestacao")]
        public int IdResultadoContestacao { get; set; }

        [Required]
        [Column("id_contestacao")]
        public int IdContestacao { get; set; }

        [Required]
        [Column("id_tpresultadocontestacao")]
        public int IdRequisito { get; set; }

        [Required]
        [Column("lgn_analista")]
        public string LoginAnalista { get; set; }

        [Column("cmn_resultado")]
        [StringLength(2000)]
        public string Observacao { get; set; }

        [Column("din_resultado")]
        public DateTime DataResultado { get; set; }

    }
}
