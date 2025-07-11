using ONS.PortalMQDI.Data.Entity;

namespace ONS.PortalMQDI.Services
{
    public interface ICalendarioCalculator
    {
        bool VerificarSeCalendarioEstaValido(CalendarioSistema calendario);
        CalendarioSistema CalculaCalendarioDoSistema();
    }
}
