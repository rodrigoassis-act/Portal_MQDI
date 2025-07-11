using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class InstalacaoRecursoRelatorioAgenteView
    {
        [Key]
        [Column("ri_id_resultadoindicador")]
        public int? RiIdResultadoIndicador { get; set; }

        [Column("nom_curto")]
        public string NomeCurto { get; set; }

        [Column("nom_longo")]
        public string NomeLongo { get; set; }

        [Column("ins_mrid")]
        public string InsMrid { get; set; }

        [Column("id_resultadoindicador")]
        public int? IdResultadoIndicador { get; set; }

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

        [Column("id_tpindicador")]
        public int IdTipoIndicador { get; set; }

        [Column("val_mensal")]
        public double? ValorMensal { get; set; }

        [Column("val_anual")]
        public double? ValorAnual { get; set; }

        [Column("flg_violacaomensal")]
        public bool ViolacaoMensal { get; set; }

        [Column("flg_violacaoanual")]
        public bool ViolacaoAnual { get; set; }

        [Column("din_recebimentoresultado")]
        public DateTime DinRecebimentoResultado { get; set; }

        [Column("id_resultadoindicadorrecalculado")]
        public int? IdResultadoIndicadorRecalculado { get; set; }

        [Column("ti_cod_tpindicador")]
        public string TipoIndicador { get; set; }

        [Column("nom_tprecurso")]
        public string NomeTipoRecurso { get; set; }

        [Column("dsc_grandeza")]
        public string DescricaoGrandeza { get; set; }

        [Column("lscinf")]
        public string Lscinf { get; set; }

        [Column("grandeza")]
        public string Grandeza { get; set; }

        [Column("tprede")]
        public string CodRede { get; set; }

        [Column("nom_enderecofisico")]
        public string EnderecoFisico { get; set; }
    }
}
