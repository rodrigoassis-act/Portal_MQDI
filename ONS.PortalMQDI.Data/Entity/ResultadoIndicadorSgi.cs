using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_resultadoindicadorsgi")]
    public class ResultadoIndicadorSgi
    {
        [Key]
        [Column("id_resultadoindicadorsgi")]
        public int IdResultadoIndicadorSgi { get; set; }

        [Required]
        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Required]
        [Column("id_sgi")]
        public int IdSgi { get; set; }
    }
}
