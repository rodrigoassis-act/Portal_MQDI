using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_usina")]
    public class Usina
    {
        [Column("idusina")]
        public int IdUsina { get; set; }

        [Column("idsistema")]
        public string IdSistema { get; set; }

        [Column("idagente")]
        public string IdAgente { get; set; }

        [Column("idtipousina")]
        public string IdTipoUsina { get; set; }

        [Column("idinstalacao")]
        public int IdInstalacao { get; set; }

        [Column("nomeusina")]
        public string NomeUsina { get; set; }

        [Column("nomeagente")]
        public string NomeAgente { get; set; }

        [Column("nomeinstalacao")]
        public string NomeInstalacao { get; set; }

        [Column("nomesistema")]
        public string NomeSistema { get; set; }

        [Column("dataalteracao")]
        public DateTime? DataAlteracao { get; set; }
    }
}
