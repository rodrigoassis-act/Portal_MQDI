using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Models.ViewModel.Filtros;

namespace ONS.PortalMQDI.Services.Services
{
    public class InstalacaoService : IInstalacaoService
    {
        private readonly IInstalacaoRepository _instalacaoRepository;
        private readonly IGrandezaRepository _grandezaRepository;
        private readonly ILogEventoService _logEventoService;
        public InstalacaoService(IInstalacaoRepository instalacaoRepository, IGrandezaRepository grandezaRepository, ILogEventoService logEventoService)
        {
            _instalacaoRepository = instalacaoRepository;
            _grandezaRepository = grandezaRepository;
            _logEventoService = logEventoService;
        }
        public async Task<List<ConsultaRecursoItemViewModel>> ConsultarRecursosAsync(ConsultarRecursosFiltroViewModel filtro, CancellationToken cancellationToke)
        {
            var instalacao = await _instalacaoRepository
                .ConsultarRecursosAsync(filtro.AnoMes, filtro.AgeMrid, filtro.IdInstalacao, filtro.IdCos, filtro.IdIncador, cancellationToke, filtro.IsContestacao);


            var recurso = new List<ConsultaRecursoItemViewModel>();

            foreach (var item in instalacao)
            {
                Enum.TryParse(item.TpRede, out TipoRedeEnum tipoRedeEnum);

                recurso.Add(new ConsultaRecursoItemViewModel
                {
                    TpRede = tipoRedeEnum.GetDescription(),
                    ConstestacaoStatus = item.ConstestacaoStatus == 1 ? true : false,
                    AgeMrid = item.AgeMrid,
                    AnoMesReferencia = item.AnoMesReferencia,
                    CodLscinf = item.CodLscinf,
                    CosId = item.CosId,
                    DinRecebimentoResultado = item.DinRecebimentoResultado,
                    DscAnalistaConstestacao = item.DscAnalistaConstestacao,
                    DscGrandeza = item.DscGrandeza,
                    DscOnsConstestacao = item.DscOnsConstestacao,
                    FlgViolacaoAnual = item.FlgViolacaoAnual,
                    FlgViolacaoMensal = item.FlgViolacaoMensal,
                    IdConstestacao = item.IdConstestacao,
                    IdoOns = item.IdoOns,
                    IdResultadoIndicador = item.IdResultadoIndicador,
                    IdResultadoIndicadorRecalculado = item.IdResultadoIndicadorRecalculado,
                    IdTpIndicador = item.IdTpIndicador,
                    IdTprecurso = item.IdTprecurso,
                    Mrid = item.Mrid,
                    NomEnderecoFisico = item.NomEnderecoFisico,
                    ValAnual = item.ValAnual,
                    ValMensal = item.ValMensal,
                    Sgi = item.SgiNumSequenciaSgi.HasValue,
                });
            }

            await _logEventoService.RegistrarEventoAsync(filtro.AgeMrid, filtro.AnoMes, PageEnum.ConsultaRecurso, cancellationToke);

            return recurso.OrderBy(c => c.IdoOns).ToList();
        }

        public async Task<List<MedidaSupervisionadaRecursoViewModel>> MedidaSupervisionadaRecursoAsync(string anoMes, string ageMrid, CancellationToken cancellationToke)
        {
            var queryInstalacao = await _grandezaRepository.InstalacaoConsultarMedidaAsync(ageMrid, anoMes.ConvertToAnomeReferencia(), cancellationToke);
            var medidas = RetornaMedidasSupervisionadasRecursos(queryInstalacao);

            await _logEventoService.RegistrarEventoAsync(ageMrid, anoMes.Replace("/", "-"), PageEnum.ConsultarMedidasNaoSupervisionadas, cancellationToke);
            return medidas;
        }

        public List<MedidaSupervisionadaRecursoViewModel> RetornaMedidasSupervisionadasRecursos(IEnumerable<InstalacaoConsultarMedidaView> queryInstalacao)
        {
            var agroupInstalacao = queryInstalacao.GroupBy(c => c.IdInstalacao).Select(c => new
            {
                InstalacaoNome = c.FirstOrDefault().NomeInstalacao,
                IdInstalacao = c.Key,
                Recurso = c.ToList()
            });

            return agroupInstalacao.Select(c => new MedidaSupervisionadaRecursoViewModel
            {
                NomeInstalacao = c.InstalacaoNome,
                IdInstalacao = c.IdInstalacao,
                Recurso = c.Recurso.Select(x => new MedidaSupervisionadaRecursoItemViewModel
                {
                    CosId = NomeCentro(x.CosId),
                    TpRede = NomeRede(x.TipoRede),
                    DscGrandeza = x.DescricaoGrandeza,
                    IdoOns = x.IdPonto
                }).ToList()
            }).ToList();
        }

        #region Auxiliares
        private string NomeCentro(string cos)
        {
            var cosIdWithoutPrefix = cos.Replace("COSR-", "");
            Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);

            return cosIdEnum.GetDescription();
        }

        private string NomeRede(string rede)
        {
            Enum.TryParse(rede, out TipoRedeEnum tipoRedeEnum);
            return tipoRedeEnum.GetDescription();
        }
        #endregion
    }
}
