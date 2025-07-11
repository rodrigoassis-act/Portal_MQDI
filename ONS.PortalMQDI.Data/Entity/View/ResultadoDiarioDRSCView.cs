using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ResultadoDiarioDRSCView
    {
        [Column("id_resultadodiario")]
        [Key]
        public int Id { get; set; }

        [Column("nom_curto")]
        public string Instalacao { get; set; }

        [Column("ido_ons")]
        public string IdoOns { get; set; }

        [Column("dsc_grandeza")]
        public string Descricao { get; set; }

        [Column("tprede")]
        public string Rede { get; set; }

        [Column("cod_lscinf")]
        public string Lscc { get; set; }

        [Column("nom_enderecofisico")]
        public string EnderecoProtocolo { get; set; }

        [Column("val_dispdiario")]
        public double DispDiaria { get; set; }

        [Column("flg_dispdiario")]
        public int FlgDispDiario { get; set; }
    }
}
