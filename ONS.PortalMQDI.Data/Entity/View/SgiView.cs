using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class SgiView
    {
        [Key]
        [Column("num_ons")]
        public string? SgiNumONS { get; set; }

        [Column("num_sequenciasgi")]
        public int? SgiNumSequenciaSgi { get; set; }

        [Column("din_inicioefetivo")]
        public DateTime? SgiInicioefetivo { get; set; }

        [Column("din_terminoefetivo")]
        public DateTime? SgiTerminoEfetivo { get; set; }
    }
}
