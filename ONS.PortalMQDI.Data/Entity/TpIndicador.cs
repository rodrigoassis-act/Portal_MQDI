using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_tpindicador")]
    public class TpIndicador
    {
        [Key]
        [Column("id_tpindicador")]
        public int Id { get; set; }

        [Column("cod_tpindicador")]
        public string CodIndicador { get; set; }

        [Column("nom_tpindicador")]
        public string Nome { get; set; }
    }
}
