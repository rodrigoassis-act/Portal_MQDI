using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONS.PortalMQDI.Data.Entity
{
    [Table("tb_equipamento")]
    public class Equipamento
    {
        [Key]
        [Column("idobjeto")]
        public int IdObjeto { get; set; }

        [Column("idtipoequipamento")]
        public string IdTipoEquipamento { get; set; }

        [Column("idsubtipoequipamento")]
        public string IdSubtipoEquipamento { get; set; }

        [Column("idagente")]
        public string IdAgente { get; set; }

        [Column("idequipamento")]
        public string IdEquipamento { get; set; }

        [Column("nomponto")]
        public string NomePonto { get; set; }

        [Column("nomeequipamento")]
        public string NomeEquipamento { get; set; }

        [Column("caminho")]
        public string Caminho { get; set; }

        [Column("caminhofisico")]
        public string CaminhoFisico { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("tipomedicao")]
        public string TipoMedicao { get; set; }

        [Column("tipofluxo")]
        public string TipoFluxo { get; set; }

        [Column("situacao")]
        public string Situacao { get; set; }

        [Column("idsubmercado")]
        public string IdSubmercado { get; set; }

        [Column("nomepontoagente")]
        public string NomePontoAgente { get; set; }

        [Column("nompontoequipamento")]
        public string NomePontoEquipamento { get; set; }

        [Column("dataalteracao")]
        public DateTime? DataAlteracao { get; set; }

        [Column("idagenteequivalente")]
        public string IdAgenteEquivalente { get; set; }

        [Column("idpontoconsolidado")]
        public int? IdPontoConsolidado { get; set; }
    }
}
