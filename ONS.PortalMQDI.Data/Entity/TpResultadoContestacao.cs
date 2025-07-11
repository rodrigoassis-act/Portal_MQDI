using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_tprescontestacao")]
    public class TpResultadoContestacao
    {
        [Key]
        [Column("id_tprescontestacao")]
        public int IdTipoResultadoContestacao { get; set; }

        [Required]
        [Column("dsc_tprescontestacao")]
        [StringLength(100)]
        public string DescricaoTipoResultadoContestacao { get; set; }
    }
}
