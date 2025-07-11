using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_indicadorresultadoexpurgo")]
    public class IndicadorResultadoExpurgo
    {
        [Key]
        [Column("id_indicadorresultadoexpurgo")]
        public int IdIndicadorResultadoExpurgo { get; set; }

        [Column("id_resultadoindicador")]
        public int IdResultadoIndicador { get; set; }

        [Column("flg_expurgoestadooperativo")]
        public char? FlagExpurgoEstadoOperativo { get; set; }

        [Column("flg_expurgosgissc")]
        public char? FlagExpurgoSGISSC { get; set; }

        [Column("flg_expurgoproblemapi")]
        public char? FlagExpurgoProblemaPI { get; set; }

        [Column("flg_expurgoutrcd")]
        public char? FlagExpurgoOutrosRCD { get; set; }
    }
}
