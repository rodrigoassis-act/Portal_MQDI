using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_grandeza")]
    public class Grandeza
    {
        [Key]
        [Column("grd_mrid")]
        public string GrdMrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("ins_mrid")]
        public string InsMrid { get; set; }

        [Column("ter_mrid")]
        public string TerMrid { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("ido_ons")]
        public string IdoOns { get; set; }

        [Column("dsc_grandeza")]
        public string DscGrandeza { get; set; }

        [Column("tip_grandeza")]
        public string TipGrandeza { get; set; }

        [Column("tprede")]
        public string TpRede { get; set; }

        [Column("cod_lscinf")]
        public string CodLscinf { get; set; }

        [Column("nom_enderecofisico")]
        public string NomEnderecoFisico { get; set; }
    }
}
