using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("TbCos")]
    public class Cos
    {
        [Key]
        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }
    }
}
