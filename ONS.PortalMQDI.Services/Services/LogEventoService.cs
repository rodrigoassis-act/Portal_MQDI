using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.IdentityModel.Tokens;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class LogEventoService : ILogEventoService
    {
        private readonly ILogEventoRepository _logEventoRepository;
        private readonly IAgenteRepository _agenteRepository;
        private readonly IUserService _userService;
        public LogEventoService(ILogEventoRepository logEventoRepository, IUserService userService, IAgenteRepository agenteRepository)
        {
            _logEventoRepository = logEventoRepository;
            _userService = userService;
            _agenteRepository = agenteRepository;
        }


        public async Task<bool> RegistrarEventoAsync(string ageMrid, string AnomesReferencia, PageEnum? page, CancellationToken cancellationToken)
        {
            var agente = new Agente();

            if(page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            
            if(ageMrid != null)
            {
                agente = await _agenteRepository.BuscarPorAgeMrid(ageMrid);
            }

            var logEvento = new LogEvento
            {
                AgeMrid = agente.AgeMrid ?? null,
                AnomesReferencia = null,
                Titulo = page.GetDescription(),
                NomeUsuario = _userService.NomeUsuario(),
                SidUsuario = _userService.SidUsuario(),
                DataEvento = DateTime.Now,
                Escopo = _userService.IsAdministrator() ? (int)TipoEscopoEnum.ONS : (int)TipoEscopoEnum.AGENTE,
                AgenteNomeCurto = ageMrid != null ? agente.NomeCurto : null,
            };

            return await _logEventoRepository.InsertAsync(logEvento, cancellationToken);
        }
    }
}
