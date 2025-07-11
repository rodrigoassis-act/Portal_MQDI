using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ContestacaoInstalacaoRecursoView
    {
        [Key]
        [Column("ins_mrid")]
        public string? InsMrid { get; set; }

        [Column("nom_curto")]
        public string? NomCurto { get; set; }

        [Column("nom_longo")]
        public string? NomLongo { get; set; }

        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Column("cos_id")]
        public string? CosId { get; set; }

        [Column("mrid")]
        public string? Mrid { get; set; }

        [Column("anomes_referencia")]
        public string? AnomesReferencia { get; set; }

        [Column("age_mrid")]
        public string? AgeMrid { get; set; }

        [Column("id_tprecurso")]
        public int IdTipoRecurso { get; set; }

        [Column("id_tpindicador")]
        public int IdTipoIndicador { get; set; }

        [Column("val_mensal")]
        public double ValMensal { get; set; }

        [Column("val_anual")]
        public double ValAnual { get; set; }

        [Column("flg_violacaomensal")]
        public bool FlgViolacaoMensal { get; set; }

        [Column("flg_violacaoanual")]
        public bool FlgViolacaoAnual { get; set; }

        [Column("din_recebimentoresultado")]
        public DateTime? DinRecebimentoResultado { get; set; }

        [Column("id_resultadoindicadorrecalculado")]
        public int? IdResultadoIndicadorRecalculado { get; set; }

        [Column("ido_ons")]
        public string? IdoOns { get; set; }

        [Column("dsc_grandeza")]
        public string? DscGrandeza { get; set; }

        [Column("tip_grandeza")]
        public string? TipGrandeza { get; set; }

        [Column("tprede")]
        public string? Tprede { get; set; }

        [Column("nom_enderecofisico")]
        public string? NomEnderecoFisico { get; set; }

        [Column("recursoAnalistaContestacao")]
        public string? RecursoAnalistaContestacao { get; set; }

        [Column("recursoOnsContestacao")]
        public string? RecursoOnsContestacao { get; set; }

        [Column("recursoOnsIdContestacao")]
        public int? RecursoOnsIdContestacao { get; set; }

        [Column("recursoOnsStatus")]
        public int? RecursoOnsStatus { get; set; }

        [Column("instalacaoAnalistaContestacao")]
        public string? InstalacaoAnalistaContestacao { get; set; }

        [Column("instalacaoOnsContestacao")]
        public string? InstalacaoOnsContestacao { get; set; }

        [Column("instalacaoOnsIdContestacao")]
        public int? InstalacaoOnsIdContestacao { get; set; }

        [Column("instalacaoOnsStatus")]
        public int? InstalacaoOnsStatus { get; set; }

        [Column("nom_tpindicador")]
        public string? NomeIndicador { get; set; }

        [Column("cod_tpindicador")]
        public string? CodIndicador { get; set; }

        [Column("cod_lscinf")]
        public string? CodLscinf { get; set; }

        [Column("recursoFragAnual")]
        public bool? RecursoFragAnual { get; set; }

        [Column("recursoFragMensal")]
        public bool? RecursoFragMensal { get; set; }

        [Column("recursoValorMensal")]
        public double? RecursoValorMensal { get; set; }

        [Column("recursoValorAnual")]
        public double? RecursoValorAnual { get; set; }

        [Column("recursoIdResultadoIndicador")]
        public int? RecursoIdResultadoIndicador { get; set; }
    }
}
