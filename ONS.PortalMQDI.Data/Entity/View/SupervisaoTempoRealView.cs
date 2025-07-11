using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity.View
{
    public class SupervisaoTempoRealView
    {
        [Column("nom_enderecofisico")]
        [Key]
        public string EnderecoProtocolo { get; set; }

        [Column("ido_ons")]
        public string CodGrandeza { get; set; }
    }
}
