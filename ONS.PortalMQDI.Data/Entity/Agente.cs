using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_agente")]
    public class Agente
    {
        [Key]
        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("nom_curto")]
        public string NomeCurto { get; set; }

        [Column("nom_longo")]
        public string NomeLongo { get; set; }

        [Column("ido_ons")]
        public string IdOns { get; set; }

    }
}
