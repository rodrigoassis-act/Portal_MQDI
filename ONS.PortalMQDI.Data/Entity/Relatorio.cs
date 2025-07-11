using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_relatorio")]
    public class Relatorio
    {
        [Key]
        [Column("id_relatorio")]
        public int IdRelatorio { get; set; }

        [Column("id_tprelatorio")]
        public int IdTpRelatorio { get; set; }

        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }

        [Column("age_mrid")]
        public string AgenteId { get; set; }

        [ForeignKey("IdTpIndicador")]
        public virtual TpIndicador TpIndicador { get; set; }

        [ForeignKey("IdTpRelatorio")]
        public virtual TpRelatorio TpRelatorio { get; set; }

        [ForeignKey("AgenteId")]
        public virtual Agente Agente { get; set; }

        [Column("anomes_referencia")]
        public string AnomesReferencia { get; set; }

        [Column("din_geracaorelatorio")]
        public DateTime GeracaoRelatorio { get; set; }
    }
}
