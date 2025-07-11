using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_requisito")]
    public class Requisito
    {
        [Key]
        [Column("id_requisito")]
        public int IdRequisito { get; set; }

        [Required]
        [Column("nom_requisito")]
        [StringLength(100)]
        public string NomeRequisito { get; set; }

        [Required]
        [Column("flg_requisitoativo")]
        public bool FlagRequisitoAtivo { get; set; }
    }
}
