using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_contestacao")]
    public class Contestacao
    {
        [Key]
        [Column("id_contestacao")]
        public int IdContestacao { get; set; }

        [Required]
        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Required]
        [Column("lgn_useragente")]
        [StringLength(50)]
        public string LoginUsuarioAgente { get; set; }

        [Required]
        [Column("dsc_contestacao")]
        [StringLength(2000)]
        public string DescricaoContestacao { get; set; }

        [ForeignKey("IdResultadoIndicador")]
        public virtual ResultadoIndicador ResultadoIndicador { get; set; }
    }
}
