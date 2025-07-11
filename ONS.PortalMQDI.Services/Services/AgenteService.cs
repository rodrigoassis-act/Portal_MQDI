using DocumentFormat.OpenXml.Office2010.Excel;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Shared.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class AgenteService : IAgenteService
    {
        private readonly IUserService _userService;
        private readonly IAgenteRepository _agenteRepository;
        private readonly ITpRelatorioRepository _tpRelatorioRepository;
        private readonly IRelatorioRepository _relatorioRepository;

        public AgenteService(IUserService userService,
            IAgenteRepository agenteRepository,
            ITpRelatorioRepository tpRelatorioRepository,
            IRelatorioRepository relatorioRepository)
        {
            _agenteRepository = agenteRepository;
            _userService = userService;
            _tpRelatorioRepository = tpRelatorioRepository;
            _relatorioRepository = relatorioRepository;
        }
        public async Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToken)
        {
            IEnumerable<Agente> listaAgentes;

            if (_userService.IsAdministrator())
            {
                listaAgentes = await _agenteRepository.TodosAgentePorGrandezaAsync(filtro.MesAnoFormatada(), cancellationToken);
            }
            else
            {
                listaAgentes = await _agenteRepository.TodosAgentePorGrandezaAsync(filtro.MesAnoFormatada(), cancellationToken, _userService.ListaEscopos());
            }

            return listaAgentes.Select(x => new SelectItemViewModel<string>
            {
                Label = x.NomeCurto.Trim(),
                Value = x.AgeMrid.Trim(),
                Title = x.NomeLongo.Trim(),
                Id=x.IdOns.Trim(),
            }).DistinctBy(c=> c.Value).OrderBy(c => c.Label).ToList();
        }

        public async Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteConsultarMedidaAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            IEnumerable<Agente> listaAgentes;

            if (_userService.IsAdministrator())
            {
                listaAgentes = await _agenteRepository
                        .BuscarAgenteConsultarMedidaAsync(filtro.MesAno.ConvertToAnomeReferencia(), null, cancellationToke);
            }
            else
            {
                listaAgentes = await _agenteRepository
                        .BuscarAgenteConsultarMedidaAsync(filtro.MesAno.ConvertToAnomeReferencia(), _userService.ListaEscopos(), cancellationToke);
            }

            return listaAgentes.Select(x => new SelectItemViewModel<string>
            {
                Label = x.NomeCurto.Trim(),
                Value = x.AgeMrid.Trim(),
                Title = x.NomeLongo.Trim(),
            }).OrderBy(c => c.Label);
        }

        public async Task<IEnumerable<SelectItemRelatorioViewModel>> BuscarAgenteDownloadRelatorioAsync(DownloadRelatorioFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            var predicate = PredicateBuilder.True<Relatorio>();

            if (!string.IsNullOrEmpty(filtro.MesAnoSelecionado))
            {
                predicate = predicate.And(p => p.AnomesReferencia == filtro.MesAnoSelecionado.ConvertToAnomeReferencia());
            }

            if (!string.IsNullOrEmpty(filtro.AgenteSelecionado))
            {
                predicate = predicate.And(p => p.AgenteId == filtro.AgenteSelecionado);
            }

            if (!string.IsNullOrEmpty(filtro.TpIndicadorSelecionado))
            {
                predicate = predicate.And(p => p.IdTpIndicador == int.Parse(filtro.TpIndicadorSelecionado));
            }

            if (filtro.TpTipoRelatorio.HasValue)
            {
                predicate = predicate.And(p => p.IdTpRelatorio == filtro.TpTipoRelatorio.Value);
            }


            if (!_userService.IsAdministrator())
            {
                predicate = predicate.And(p => p.IdTpIndicador !=  (int)TipoRelatorioEnum.RAmD);
            }

            predicate = predicate.And(p => p.IdTpIndicador != (int)TipoRelatorioEnum.RAvDQ && p.IdTpIndicador != (int)TipoRelatorioEnum.RAcDQ);

            var relatorioAgente = await _relatorioRepository.GetAsync(predicate, cancellationToke);

            return relatorioAgente.OrderByDescending(c => c.AnomesReferencia)
                .Select(x => new SelectItemRelatorioViewModel
                {
                    Label = x.AnomesReferencia.Replace("-", "/"),
                    Value = !string.IsNullOrEmpty(x.AgenteId) ? x.AgenteId.Trim() : null,
                    IdTipoIndicador = x.IdTpIndicador,
                    IdTipoRelatorio = x.IdTpRelatorio,
                });
        }

        public async Task<IEnumerable<SelectItemViewModel<string>>> BuscarAgenteIndicadorAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            IEnumerable<Agente> listaAgentes;

            if (_userService.IsAdministrator())
            {
                listaAgentes = await _agenteRepository
                        .BuscarAgenteIndicadorAsync(filtro.MesAno.ConvertToAnomeReferencia(), null, cancellationToke);
            }
            else
            {
                listaAgentes = await _agenteRepository
                        .BuscarAgenteIndicadorAsync(filtro.MesAno.ConvertToAnomeReferencia(), _userService.ListaEscopos(), cancellationToke);
            }

            return listaAgentes.Select(x => new SelectItemViewModel<string>
            {
                Label = x.NomeCurto.Trim(),
                Value = x.AgeMrid.Trim(),
                Title = x.NomeLongo.Trim(),
            }).OrderBy(c => c.Label);
        }

        public async Task<IEnumerable<SelectItemRelatorioViewModel>> BuscarAgenteRelatorioAsync(AgenteFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            var predicate = PredicateBuilder.True<Relatorio>();

            if (!string.IsNullOrEmpty(filtro.MesAno))
            {
                predicate = predicate.And(p => p.AnomesReferencia == filtro.MesAno.ConvertToAnomeReferencia());
            }

            if (!_userService.IsAdministrator())
            {
                predicate = predicate.And(p => p.IdTpIndicador != (int)TipoRelatorioEnum.RAmD && _userService.ListaEscopos().Contains(p.Agente.IdOns.Trim()));
            }

            var relatorioAgente = await _relatorioRepository.BuscarAgenteAsync(predicate, cancellationToke);

            var distinctRelatorio = relatorioAgente.GroupBy(x => (x.AnomesReferencia, x.AgenteId))
                                       .Select(g => g.First())
                                       .ToList();


            return distinctRelatorio
                .Where(c => c.Agente != null)
                .Select(x => new SelectItemRelatorioViewModel
                {
                    Label = x.Agente.NomeCurto.Trim(),
                    Value = x.Agente.AgeMrid.Trim(),
                    Title = x.Agente.NomeLongo.Trim(),
                    TipoIndicador = x.TpIndicador.CodIndicador,
                    TipoRelatorio = x.IdTpRelatorio,
                    IdOns = x.Agente.IdOns.Trim(),
                    AnoMes = x.AnomesReferencia.ConvertAnomeReferenciaToDate()
                }).OrderBy(c => c.Label);
        }

        public async Task<IEnumerable<SelectItemViewModel<int>>> BuscarTipoRelatorioAsync(CancellationToken cancellationToke)
        {

            var tipoRelatorio = await _tpRelatorioRepository.GetAllAsync(cancellationToke);

            tipoRelatorio = tipoRelatorio.Where(c => c.Codigo != TipoRelatorioEnum.RAvDQ.GetDescription() && c.Codigo != TipoRelatorioEnum.RAcDQ.GetDescription());


            if (!_userService.IsAdministrator())
            {
                tipoRelatorio = tipoRelatorio.Where(c => c.Codigo != TipoRelatorioEnum.RAmD.GetDescription());
            }



            return tipoRelatorio.Select(x => new SelectItemViewModel<int>
            {
                Label = x.Codigo,
                Value = x.IdTpRelatorio,
                Title = x.Nome,

            }).OrderBy(c => c.Value);
        }
    }
}
