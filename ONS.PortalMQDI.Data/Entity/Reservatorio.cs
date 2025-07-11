using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_reservatorio")]
    public class Reservatorio
    {
        [Column("idreservatorio")]
        public int IdReservatorio { get; set; }

        [Column("idsistema")]
        public string IdSistema { get; set; }

        [Column("idponto")]
        public string IdPonto { get; set; }

        [Column("nomeponto")]
        public string NomePonto { get; set; }

        [Column("idinstalacao")]
        public int IdInstalacao { get; set; }

        [Column("nomeinstalacao")]
        public string NomeInstalacao { get; set; }

        [Column("nomesistema")]
        public string NomeSistema { get; set; }

        [Column("dataalteracao")]
        public DateTime? DataAlteracao { get; set; }
    }
}
