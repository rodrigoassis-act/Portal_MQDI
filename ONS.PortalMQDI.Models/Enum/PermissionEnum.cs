using System.ComponentModel;

namespace ONS.PortalMQDI.Models.Enum
{
    public enum PermissionEnum
    {
        [Description("ONS/ONS|#$%MQDI_Monitoramento TR")]
        OnsMonitoramentoTR,
        [Description("ONS/ONS|#$%MQDI_Analista ONS")]
        OnsAnalista,
        [Description("ONS/ONS|#$%MQDI_Administrador")]
        Administrator,
        [Description("PortalMQDI")]
        PortalMQDI,
        [Description("MQDI_Alterar parâmetros do sistema")]
        MQDIAlterarParametrosSistema,
        [Description("MQDI_Criar Contestações sobre os indicadores")]
        MQDICriarContestacao,
        [Description("MQDI_Responder a contestações dos agentes")]
        MQDIResponderContestacao
    }
}
