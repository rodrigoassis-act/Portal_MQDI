using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.ViewModel
{
    public class ResultadoIndicadorViewModel
    {
        public List<ResultadoIndicadorTableViewModel> Agente { get; set; }
        public List<ResultadoIndicadorTableViewModel> SSCL { get; set; }
        public List<ResultadoIndicadorTableViewModel> Instalacao { get; set; }
    }

    public class ResultadoIndicadorTableViewModel
    {
        public string DscAnalista { get; set; }
        public string DscAnalistaONS { get; set; }
        public int IdResultadoIndicador { get; set; }
        public string Cos { get; set; }
        public string ValueName { get; set; }
        public string Indicador { get; set; }
        public ItemTableViewModel Anual { get; set; }
        public ItemTableViewModel Mensal { get; set; }
        public string IdInstalacao { get; set; }
        public List<ItemRecusoTableViewModel> Recurso { get; set; }
        public string IdCos { get; set; }
        public int IdIncador { get; set; }
        public int? IdConstestacao { get; set; }
        public bool? ConstestacaoStatus { get; set; }
        public ResultadoIndicadorTableViewModel()
        {
            Anual = new ItemTableViewModel();
            Mensal = new ItemTableViewModel();
            Recurso = new List<ItemRecusoTableViewModel>();
        }
    }

    public class ItemTableViewModel
    {
        public string Valor { get; set; }
        public bool Violacao { get; set; }
    }


    public class ItemRecusoTableViewModel
    {
        public int IdResultadoIndicador { get; set; }
        public string NomeFisico { get; set; }
        public string CodLscinf { get; set; }
        public string CodOns { get; set; }
        public string Indicador { get; set; }
        public string Descricao { get; set; }
        public string CostentacaoAgente { get; set; }
        public string CostentacaoOns { get; set; }
        public bool ConstestacaoStatus { get; set; }
        public int IdConstatacao { get; set; }
        public string Rede { get; set; }
        public ItemTableViewModel Anual { get; set; }
        public ItemTableViewModel Mensal { get; set; }

        public ItemRecusoTableViewModel()
        {
            Anual = new ItemTableViewModel();
            Mensal = new ItemTableViewModel();
        }
    }
}
