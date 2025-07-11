using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_parametro")]
    public class Parametro
    {
        [Key]
        [Column("id_parametro")]
        public int IdParametro { get; set; }

        [Required]
        [Column("tip_parametro")]
        [StringLength(8)]
        public string TipoParametro { get; set; }

        [Required]
        [Column("nom_parametro")]
        [StringLength(100)]
        public string NomeParametro { get; set; }
    }
}
