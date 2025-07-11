using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_logevento")]
    public class LogEvento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_logevento")]
        public int IdLogEvento { get; set; }

        [Required]
        [Column("age_mrid")]
        public string AgeMrid { get; set; }

        [Column("anomes_referencia")]
        public string AnomesReferencia { get; set; }

        [Column("din_evento")]
        public DateTime DataEvento { get; set; }

        [Column("cod_sidusuario")]
        public string SidUsuario { get; set; }

        [Column("nom_usuario")]
        public string NomeUsuario { get; set; }

        [Column("dsc_titulo")]
        public string Titulo { get; set; }

        [Column("tp_escopo")]
        public int? Escopo { get; set; } 

        [Column("age_nomecurto")]
        public string AgenteNomeCurto { get; set; } 
    }
}
