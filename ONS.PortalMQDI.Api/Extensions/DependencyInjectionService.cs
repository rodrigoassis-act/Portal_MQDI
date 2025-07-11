using Microsoft.Extensions.DependencyInjection;
using ONS.PortalMQDI.Services;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Services.Services;

namespace ONS.PortalMQDI.Api.Extensions
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddDIService(this IServiceCollection services)
        {
            services.AddScoped<JwtService>();
            services.AddScoped<NotificacaoService>();
            services.AddScoped<EmailService>();
            services.AddScoped<SharepointService>(); // Temporario, remover a proxima sprint 
            services.AddScoped<IMigrationService, MigrationService>(); // Temporario, remover a proxima sprint 
            services.AddScoped<ICalendarioService, CalendarioService>();
            services.AddScoped<IParametroSistemaService, ParametroSistemaService>();
            services.AddScoped<IAgenteService, AgenteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResultadoIndicadorService, ResultadoIndicadorService>();
            services.AddScoped<IAcompanhamentoGeralIndicadorService, AcompanhamentoGeralIndicadorService>();
            services.AddScoped<IInstalacaoService, InstalacaoService>();
            services.AddScoped<ICalendarioService, CalendarioService>();
            services.AddScoped<ISupervisaoTempoRealService, SupervisaoTempoRealService>();
            services.AddScoped<IExportarService, ExportarService>();
            services.AddScoped<ICargaService, CargaService>();
            services.AddScoped<ICalendarioCalculator, CalendarioCalculator>();
            services.AddScoped<IContestacaoService, ContestacaoService>();
            services.AddScoped<IRelatorioService, RelatorioService>();
            services.AddScoped<IResultadoDiarioService, ResultadoDiarioService>();
            services.AddScoped<IAwsService, AwsService>();
            services.AddScoped<ILogEventoService, LogEventoService>();
         
            return services;
        }
    }
}
