using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class CalendarioDataExisteView
    {
        [Key]
        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }
    }
}
