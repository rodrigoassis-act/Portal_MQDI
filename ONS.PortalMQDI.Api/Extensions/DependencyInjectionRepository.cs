using Microsoft.Extensions.DependencyInjection;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Repositories;
using ONS.PortalMQDI.Data.Repositories.Single;
using ONS.PortalMQDI.Data.Repository;

namespace ONS.PortalMQDI.Api.Extensions
{
    public static class DependencyInjectionRepository
    {
        public static IServiceCollection AddDIRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<CargaRepository>();
            services.AddScoped<IAnalistaCosRepository, AnalistaCosRepository>();
            services.AddScoped<ICalendarioSistemaRepository, CalendarioSistemaRepository>();
            services.AddScoped<IContestacaoRepository, ContestacaoRepository>();
            services.AddScoped<ICosRepository, CosRepository>();
            services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
            services.AddScoped<IGrandezaRepository, GrandezaRepository>();
            services.AddScoped<IIndicadorRepository, IndicadorRepository>();
            services.AddScoped<IIndicadorResultadoExpurgoRepository, IndicadorResultadoExpurgoRepository>();
            services.AddScoped<IInstalacaoRepository, InstalacaoRepository>();
            services.AddScoped<IParametroRepository, ParametroRepository>();
            services.AddScoped<IRecursoRepository, RecursoRepository>();
            services.AddScoped<IRelatorioRepository, RelatorioRepository>();
            services.AddScoped<IRequisitoRepository, RequisitoRepository>();
            services.AddScoped<IReservatorioRepository, ReservatorioRepository>();
            services.AddScoped<IResultadoContestacaoRepository, ResultadoContestacaoRepository>();
            services.AddScoped<IResultadoDiarioRepository, ResultadoDiarioRepository>();
            services.AddScoped<IResultadoIndicadorRepository, ResultadoIndicadorRepository>();
            services.AddScoped<IResultadoIndicadorSgiRepository, ResultadoIndicadorSgiRepository>();
            services.AddScoped<ISgiRepository, SgiRepository>();
            services.AddScoped<ITpRelatorioRepository, TpRelatorioRepository>();
            services.AddScoped<ITpResultadoContestacaoRepository, TpResultadoContestacaoRepository>();
            services.AddScoped<IUsinaRepository, UsinaRepository>();
            services.AddScoped<IUtrCdRepository, UtrCdRepository>();
            services.AddScoped<IAnalistaCosRepository, AnalistaCosRepository>();
            services.AddScoped<IValorParametroSistemaRepository, ValorParametroSistemaRepository>();
            services.AddScoped<IAgenteRepository, AgenteRepository>();
            services.AddScoped<ITpIndicadorRepository, TpIndicadorRepository>();
            services.AddScoped<IFeriadoRepository, FeriadoRepository>();
            services.AddScoped<ILogEventoRepository, LogEventoRepository>();

            return services;
        }
    }
}
