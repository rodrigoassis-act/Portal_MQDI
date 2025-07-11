using System.ComponentModel;

namespace ONS.PortalMQDI.Models.Enum
{
    public enum ClaimsEnum
    {
        [Description("http://schemas.xmlsoap.org/ws/2015/07/identity/claims/operation")]
        Operation,
        [Description("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
        Role,
        [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
        Nameidentifier,
        [Description("http://schemas.xmlsoap.org/ws/2019/01/Sintegre/claims/publico")]
        Publico,
        [Description("aud")]
        Aud,
        [Description("http://schemas.xmlsoap.org/ws/2015/07/identity/claims/scoperole")]
        ScopeRole,
        [Description("exp")]
        Exp,
        [Description("http://schemas.xmlsoap.org/ws/2015/07/identity/claims/scope")]
        Scope,
        [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid")]
        Sid,
        [Description("given_name")]
        GivenName,
    }
}
