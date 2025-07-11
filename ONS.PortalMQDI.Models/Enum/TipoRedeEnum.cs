using System.ComponentModel;

namespace ONS.PortalMQDI.Models.Enum
{
    public enum TipoRedeEnum
    {
        [Description("BASICA")]
        BAS,

        [Description("COMPLEMENTAR")]
        COM,

        [Description("DP CENTRALIZADO")]
        UDC,

        [Description("Em branco")]
        NONE,

        [Description("FICTICIO")]
        FIC,

        [Description("SIMULACAO")]
        SIM,

        [Description("SISTEMA ISOLADO")]
        ISO,

        [Description("SUPERVISAO")]
        SUP
    }
}
