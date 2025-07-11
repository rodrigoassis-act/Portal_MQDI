using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class ExportarInstalacaoView
    {
        [Key]
        [Column("ri_id_resultadoindicador")]
        public int? IdResultadoIndicador { get; set; }

        [Column("nom_curto")]
        public string NomeCurtoInstalacao { get; set; }

        [Column("nom_longo")]
        public string NomeLongoInstalacao { get; set; }

        [Column("ti_cod_tpindicador")]
        public string TipoIndicador { get; set; }

        [Column("cos_id")]
        public string CosId { get; set; }

        [Column("instalacaoValorAnual")]
        public double? ValorInstalacaoAnual { get; set; }

        [Column("instalacaoValorMensal")]
        public double? ValorInstalacaoMensal { get; set; }

        [Column("recursoValorAnual")]
        public double? ValorRecursoAnual { get; set; }

        [Column("recursoValorMensal")]
        public double? ValorRecursoMensal { get; set; }

        [Column("dsc_grandeza")]
        public string DescricaoGrandeza { get; set; }

        [Column("lscinf")]
        public string Lscinf { get; set; }

        [Column("grandeza")]
        public string Grandeza { get; set; }

        [Column("e_tprede")]
        public string CodRede { get; set; }

        [Column("nom_enderecofisico")]
        public string NomEnderecoFisico { get; set; }

        [Column("recursoAnalista")]
        public string RecursoAnalista { get; set; }

        [Column("recursoONS")]
        public string RecursoONS { get; set; }

        [Column("recursoStatusContestacao")]
        public int? RecursoStatusContestacao { get; set; }

        [Column("instalacaoAnalista")]
        public string InstalacaoAnalista { get; set; }

        [Column("instalacaoONS")]
        public string InstalacaoONS { get; set; }

        [Column("instalacaoStatusContestacao")]
        public int? InstalacaoStatusContestacao { get; set; }

        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("instalacaoFragAnual")]
        public bool InstalacaoFragAnual { get; set; }

        [Column("instalacaoFragMensal")]
        public bool InstalacaoFragMensal { get; set; }

        [Column("recursoFragAnual")]
        public bool? RecursoFragAnual { get; set; }

        [Column("recursoFragMensal")]
        public bool? RecursoFragMensal { get; set; }
    }
}
