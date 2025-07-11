using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("TbTpIndicador")]
    public class Indicador
    {
        [Key]
        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }
    }
}
