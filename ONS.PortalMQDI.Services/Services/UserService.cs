using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ONS.PortalMQDI.Services.Services
{
    public class UserService : IUserService
    {
        private readonly JwtService jwtService;
        public UserService(JwtService jwtService)
        {
            this.jwtService = jwtService;
        }

        public string Authorization()
        {
            return jwtService.Authorization();
        }

        public bool CheckOperation(PermissionEnum permissoe)
        {
            return jwtService.VerrificarOperacao(permissoe);
        }

        public bool IsAdministrator()
        {

            PermissionEnum[] _permissions;
            return jwtService.IsAdministrator();
        }

        public List<string> ListaEscopos()
        {
            return jwtService.ListaEscopos();
        }

        public List<string> ListaOperacao()
        {
            return jwtService.ListaOperacao();
        }

        public string LoginUsuario()
        {
            var usuario = jwtService.GetClaimFromJwt().FirstOrDefault(c => c.Type == ClaimsEnum.Nameidentifier.GetDescription());
            return usuario != null ? usuario.Value : string.Empty;
        }

        public string NomeUsuario()
        {
            var usuario = jwtService.GetClaimFromJwt().FirstOrDefault(c => c.Type == ClaimsEnum.GivenName.GetDescription());
            return usuario != null ? usuario.Value : string.Empty;
        }

        public bool PodeEditarParametrosSistema()
        {
            var possuiPermissao = jwtService.PodeEditarParametrosSistema(PermissionEnum.MQDIAlterarParametrosSistema);
            return possuiPermissao;
        }

        public string SidUsuario()
        {
            return jwtService.SidUsuario();
        }

        public bool VerificaSeUsuarioPodeEditarParametrosSistema()
        {
            throw new System.NotImplementedException();
        }
    }
}
