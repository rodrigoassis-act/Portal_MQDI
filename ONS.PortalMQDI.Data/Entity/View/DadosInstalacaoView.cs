using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class DadosInstalacaoView
    {
        [Column("IdAgente")]
        [Key]
        public string IdAgente { get; set; }

        [Column("NomeAgente")]
        public string NomeAgente { get; set; }

        [Column("CentroOperacao")]
        public string CentroOperacao { get; set; }

        [Column("NomeInstalacao")]
        public string NomeInstalacao { get; set; }

        [Column("IdInstalacao")]
        public string IdInstalacao { get; set; }

        [Column("IndiceAnual")]
        public double? IndiceAnual { get; set; }

        [Column("FlagViolacaoAnual")]
        public bool FlagViolacaoAnual { get; set; }

        [Column("Id_ponto")]
        public string IdPonto { get; set; }

        [Column("Endereco")]
        public string Endereco { get; set; }

        [Column("UTRCD")]
        public string UTRCD { get; set; }

        [Column("Disponibilidade")]
        public double Disponibilidade { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("ExpurgoEstadoOperativo")]
        public string ExpurgoEstadoOperativo { get; set; }

        [Column("ExpurgoHistoricoPI")]
        public string ExpurgoHistoricoPI { get; set; }
    }
}
