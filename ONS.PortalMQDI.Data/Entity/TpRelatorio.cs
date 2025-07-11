using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_tprelatorio")]
    public class TpRelatorio
    {
        [Key]
        [Column("id_tprelatorio")]
        public int IdTpRelatorio { get; set; }

        [Column("cod_tprelatorio")]
        public string Codigo { get; set; }

        [Column("nom_tprelatorio")]
        public string Nome { get; set; }
    }
}
