using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Models.Model;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IRelatorioService
    {
        void GerarRMCViolacoesQualidade(AutomacaoIndicador automacao);
        void GerarRMCAcompanhamentoQualidade(AutomacaoIndicador automacao);
        void GerarRMIndicadoresQualidade(AutomacaoIndicador automacao);
        void GerarRMManutençãoDisponibildade(List<DadosInstalacaoView> grandezaEntry, string anoMes);
    }
}
