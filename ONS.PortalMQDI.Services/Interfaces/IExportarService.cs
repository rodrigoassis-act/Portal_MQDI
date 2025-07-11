using Microsoft.AspNetCore.Mvc;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IExportarService
    {
        FileContentResult ExportaMedidaNaoSupervisionada(ExportarConsultaIndicadorViewModel filtro);
        FileContentResult ExportarConsultaindicadores(ExportarConsultaIndicadorViewModel filtro);
        List<string> ExportaRelatorioAnalitico(ExportarConsultaIndicadorViewModel filtro);
    }
}
