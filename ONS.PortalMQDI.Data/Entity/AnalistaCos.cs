using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_analistacos")]
    public class AnalistaCos
    {
        [Key]
        [Column("id_analistacos")]
        public int IdAnalistaCos { get; set; }

        [Required]
        [Column("lgn_analista")]
        [StringLength(50)]
        public string LoginAnalista { get; set; }

        [Required]
        [Column("cos_id")]
        [StringLength(2)]
        public string CodigoCos { get; set; }
    }
}
