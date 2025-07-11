using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ContestacaoAgenteView
    {
        [Key]
        [Column("nom_curto")]
        public string NomeCurto { get; set; }

        [Column("anomes_referencia")]
        public string AnoMes { get; set; }

        [Column("nom_longo")]
        public string NomeLongo { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("mrid")]
        public string Mrid { get; set; }
    }
}
