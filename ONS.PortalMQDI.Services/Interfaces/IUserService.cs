using ONS.PortalMQDI.Models.Enum;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IUserService
    {
        string LoginUsuario();
        List<string> ListaEscopos();
        bool IsAdministrator();
        bool PodeEditarParametrosSistema();
        bool VerificaSeUsuarioPodeEditarParametrosSistema();
        bool CheckOperation(PermissionEnum permissoes);
        string Authorization();
        string SidUsuario();
        List<string> ListaOperacao();
        string NomeUsuario();
    }
}
