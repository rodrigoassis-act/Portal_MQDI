using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_aux_feriado")]
    public class Feriado
    {
        [Key]
        [Column("feriado_id")]
        public int FeriadoId { get; set; }

        [Column("nome")]
        public string NomeFeriado { get; set; }

        [Column("Data")]
        public DateTime? DataFeriado { get; set; }

        [Column("estad_id")]
        public string EstadoId { get; set; }
    }
}
