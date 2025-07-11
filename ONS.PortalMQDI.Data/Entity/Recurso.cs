using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_tprecurso")]
    public class Recurso
    {
        [Key]
        [Column("id_tprecurso")]
        public int IdTipoRecurso { get; set; }

        [Required]
        [Column("dsc_tprecurso")]
        [StringLength(200)]
        public string DescricaoTipoRecurso { get; set; }
    }
}
