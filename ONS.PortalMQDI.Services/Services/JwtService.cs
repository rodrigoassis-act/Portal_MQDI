using Microsoft.AspNetCore.Http;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace ONS.PortalMQDI.Services.Services
{
    public class JwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string BearerPrefix = "Bearer ";

        public JwtService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IEnumerable<Claim> GetClaimFromJwt()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorization)
                && authorization.ToString().StartsWith(BearerPrefix))
            {
                var bearer = authorization.ToString().Substring(BearerPrefix.Length).Trim();
                var jwt = new JwtSecurityTokenHandler();

                if (jwt.CanReadToken(bearer))
                {
                    var tokens = jwt.ReadJwtToken(bearer);

                    if (IsTokenExpired(tokens.Claims))
                    {
                        throw new AuthenticationException("O token JWT expirou.");
                    }
                    return tokens.Claims;
                }
            }

            return Enumerable.Empty<Claim>();
        }

        private bool IsTokenExpired(IEnumerable<Claim> Clains)
        {
            var expirationClaim = Clains.FirstOrDefault(c => c.Type == "exp");

            if (expirationClaim == null)
            {
                throw new AuthenticationException("O token não tem uma reivindicação de expiração.");
            }

            if (!long.TryParse(expirationClaim.Value, out var unixTime))
            {
                throw new AuthenticationException("Tempo de expiração inválido no token.");
            }

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            return DateTime.UtcNow > expirationTime;
        }

        public bool CheckPermission(PermissionEnum[] permissions)
        {


            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Role.GetDescription() || c.Type == ClaimsEnum.Aud.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            if (filterClaim.Count > 1)
            {
                return permissions.Any(item => filterClaim.Any(c => item.GetDescription().Contains(c.Claims)));
            }

            return false;
        }

        public bool CheckPermission(PermissionEnum permissions)
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Role.GetDescription() || c.Type == ClaimsEnum.Aud.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            if (filterClaim.Count > 1)
            {
                return filterClaim.Any(c => c.Claims == permissions.GetDescription());
            }

            return false;
        }

        public List<string> ListaEscopos()
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Scope.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            return filterClaim.Where(c => c.Claims != "ONS/ONS").Select(c => c.Claims.Substring(c.Claims.LastIndexOf('/') + 1)).ToList();
        }

        public List<string> ListaOperacao()
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Operation.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            return filterClaim.Where(c => c.Claims != "ONS/ONS").Select(c => c.Claims.Substring(c.Claims.LastIndexOf('/') + 1)).ToList();
        }

        public bool IsAdministrator()
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Scope.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            return filterClaim.Any(c => c.Claims == "ONS/ONS");
        }

        public bool PodeEditarParametrosSistema(PermissionEnum operation)
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Operation.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            return filterClaim.Any(c => c.Claims == operation.GetDescription());
        }

        public bool VerrificarOperacao(PermissionEnum permission)
        {
            var claims = GetClaimFromJwt();
            var operaco = new List<string>();
            var filterClaim = claims
                .Where(c => c.Type == ClaimsEnum.Operation.GetDescription())
                .Select(c => new { Claim = c.Issuer, Claims = c.Value })
                .ToList();

            return filterClaim.Any(c => c.Claims == permission.GetDescription());
        }


        public string Authorization()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationValue))
            {
                return authorizationValue.ToString().Replace("Bearer ", "");
            }
            return null;
        }

        internal string SidUsuario()
        {
            var claims = GetClaimFromJwt();

            var filterClaim = claims
                 .FirstOrDefault(c => c.Type == ClaimsEnum.Sid.GetDescription());


            if (filterClaim != null)
            {
                return filterClaim.Value;
            }

            return null;
        }
    }
}
