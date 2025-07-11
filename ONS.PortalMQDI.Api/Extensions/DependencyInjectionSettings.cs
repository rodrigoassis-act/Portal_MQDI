using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONS.PortalMQDI.Shared.Settings;

public static class DependencyInjectionSettings
{
    public static IServiceCollection AddDISettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PopServiceSettings>(configuration.GetSection("PopService"));
        services.Configure<ServiceGlobalSettings>(configuration.GetSection("ServiceGlobal"));
        services.Configure<SharepointSettings>(configuration.GetSection("Sharepoint"));
        services.Configure<ConfigSettings>(configuration.GetSection("Config"));
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.Configure<AwsSettings>(configuration.GetSection("Aws"));

        return services;
    }
}
