using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_valparametrosistema")]
    public class ValorParametroSistema
    {
        [Key]
        [Column("id_valparametro")]
        public int IdParametro { get; set; }

        [Column("id_parametro")]
        public int IdParametroSistema { get; set; }

        [Column("val_parametro")]
        public string ValParametro { get; set; }


        [Column("din_fimvigencia")]
        public DateTime? DataFimVigencia { get; set; }

        [ForeignKey("IdParametroSistema")]
        public virtual Parametro ParametroSistema { get; set; }
    }
}
