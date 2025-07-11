using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_calendariosistema")]
    public class CalendarioSistema
    {
        [Key]
        [Column("id_calendariosistema")]
        public int IdCalendarioSistema { get; set; }

        [Column("din_liberacaocontestacoesagente")]
        public DateTime DataLiberacaoContestacoesAgente { get; set; }

        [Column("din_terminoperiodocontestacoesagente")]
        public DateTime DataTerminoPeriodoContestacoesAgente { get; set; }

        [Column("din_terminoperiodoanalisecontestacoes")]
        public DateTime DataTerminoPeriodoAnaliseContestacoes { get; set; }

        [Column("anomes_referencia")]
        public string DataReferenciaIndicador { get; set; }
    }
}
