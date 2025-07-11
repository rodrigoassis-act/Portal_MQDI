using ONS.PortalMQDI.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface ILogEventoService
    {
        Task<bool> RegistrarEventoAsync(string ageMrid, string AnomesReferencia, PageEnum? page, CancellationToken cancellationToken);
    }
}
