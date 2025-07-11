using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ONS.PortalMQDI.Data.Entity.View
{
    public class InstalacaoConsultarMedidaView
    {
        [Key]
        [Column("IdInstalacao")]
        public string IdInstalacao { get; set; }

        [Column("Instalacao")]
        public string NomeInstalacao { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("IdPonto")]
        public string IdPonto { get; set; }

        [Column("dsc_grandeza")]
        public string DescricaoGrandeza { get; set; }

        [Column("tprede")]
        public string TipoRede { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }
    }
}
