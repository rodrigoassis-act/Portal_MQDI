using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class AgenteIndicadorView
    {
        [Key]
        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Column("nom_tprecurso")]
        public string NomeTipoRecurso { get; set; }

        [Column("nom_curto")]
        public string NomeCurto { get; set; }

        [Column("nom_longo")]
        public string NomeLongo { get; set; }

        [Column("ido_ons")]
        public string IdOns { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("mrid")]
        public string Mrid { get; set; }

        [Column("anomes_referencia")]
        public string AnoMesReferencia { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("id_tprecurso")]
        public int IdTipoRecurso { get; set; }



        [Column("val_mensal")]
        public double ValMensal { get; set; }

        [Column("val_anual")]
        public double ValAnual { get; set; }

        [Column("flg_violacaomensal")]
        public bool FlgViolacaoMensal { get; set; }

        [Column("flg_violacaoanual")]
        public bool FlgViolacaoAnual { get; set; }

        [Column("din_recebimentoresultado")]
        public DateTime DinRecebimentoResultado { get; set; }

        [Column("id_resultadoindicadorrecalculado")]
        public int? IdResultadoIndicadorRecalculado { get; set; }

        [Column("id_tpindicador")]
        public int IdTpIndicador { get; set; }


        [Column("cod_tpindicador")]
        public string CodIndicador { get; set; }

        [Column("nom_tpindicador")]
        public string NomeIndicador { get; set; }
    }
}
