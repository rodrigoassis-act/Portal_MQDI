using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ScadaView
    {
        [Column("ido_ons")]
        [Key]
        public string IdoOns { get; set; }

        [Column("cod_lscinf")]
        public string CodLscinf { get; set; }

        [Column("age_grandeza")]
        public string AgeMrid { get; set; }

        [Column("agedono")]
        public string AgeDono { get; set; }
    }
}
