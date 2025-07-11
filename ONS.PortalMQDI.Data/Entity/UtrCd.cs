using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_utrcd")]
    public class UtrCd
    {
        [Key]
        [Column("utrcd_mrid")]
        public string IdUtr { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("ido_ons")]
        public string IdoOns { get; set; }

        [Column("nom_utrcd")]
        public string NomeUtrcd { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("cod_lscinf")]
        public string CodLscinf { get; set; }
    }
}
