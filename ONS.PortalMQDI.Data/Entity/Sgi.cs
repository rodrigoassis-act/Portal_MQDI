using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_sgi")]
    public class Sgi
    {
        [Key]
        [Column("id_sgi")]
        public int IdSgi { get; set; }

        [Required]
        [Column("nom_sgi")]
        [StringLength(200)]
        public string NomeSgi { get; set; }
    }
}
