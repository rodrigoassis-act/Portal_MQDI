using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_resultadoindicador")]
    public class ResultadoIndicador
    {
        [Key]
        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("mrid")]
        public string Mrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("age_mrid")]
        public string? AgeMrid { get; set; }

        [Column("id_tprecurso")]
        public int IdTipoRecurso { get; set; }

        [Column("id_tpindicador")]
        public int IdTipoIndicador { get; set; }

        [Column("val_mensal")]
        public double ValorMensal { get; set; }

        [Column("val_anual")]
        public double ValorAnual { get; set; }

        [Column("flg_violacaomensal")]
        public bool FlgViolacaoMensal { get; set; }

        [Column("flg_violacaoanual")]
        public bool FlgViolacaoAnual { get; set; }

        [Column("din_recebimentoresultado")]
        public DateTime DataRecebimentoResultado { get; set; }

        [Column("id_resultadoindicadorrecalculado")]
        public int? IdResultadoIndicadorRecalculado { get; set; }

        [ForeignKey("AgeMrid")]
        public virtual Agente? Agente { get; set; }
    }
}
