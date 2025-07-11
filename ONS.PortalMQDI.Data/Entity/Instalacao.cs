using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_instalacao")]
    public class Instalacao
    {
        [Key]
        [Column("ins_mrid")]
        public string IdInstalacao { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("ido_ons")]
        public string IdoOns { get; set; }

        [Column("nom_curto")]
        public string NomeCurto { get; set; }

        [Column("nom_longo")]
        public string NomeLongo { get; set; }

        public virtual List<ResultadoIndicador> ResultadoIndicador { get; set; }
    }
}
