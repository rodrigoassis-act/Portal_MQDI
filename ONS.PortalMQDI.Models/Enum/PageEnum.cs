using System.ComponentModel;

namespace ONS.PortalMQDI.Models.Enum
{
    public enum PageEnum
    {
        [Description("Acompanhamento Geral de Indicadores")]
        AcompanhamentoGeralDeIndicadores,

        [Description("Calendário do Sistema")]
        CalendarioDoSistema,

        [Description("Consulta de Indicadores")]
        ConsultaDeIndicadores,

        [Description("Consulta Recurso")]
        ConsultaRecurso,

        [Description("Consultar Medidas Não Supervisionadas")]
        ConsultarMedidasNaoSupervisionadas,

        [Description("Relatórios Mensais")]
        RelatoriosMensais,

        [Description("Supervisão em Tempo Real")]
        SupervisaoEmTempoReal,

        [Description("Consulta de Contestações")]
        ConsultaDeContestacoes,

        [Description("Consulta de Indicadores Diário")]
        ConsultaDeIndicadoresDiario
    }
}
