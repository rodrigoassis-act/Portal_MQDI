using Microsoft.EntityFrameworkCore;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Entity.View;
using System;

namespace ONS.PortalMQDI.Data.Context
{
    public class PortalMQDIDbContext : DbContext
    {
        public DbSet<Agente> Agente { get; set; }
        public DbSet<AnalistaCos> AnalistasCos { get; set; }
        public DbSet<CalendarioSistema> CalendariosSistema { get; set; }
        public DbSet<Contestacao> Contestacoes { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Feriado> Feriado { get; set; }
        public DbSet<Grandeza> Grandezas { get; set; }
        public DbSet<IndicadorResultadoExpurgo> IndicadoresResultadoExpurgo { get; set; }
        public DbSet<Instalacao> Instalacao { get; set; }
        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Requisito> Requisitos { get; set; }
        public DbSet<ResultadoContestacao> ResultadosContestacao { get; set; }
        public DbSet<ResultadoDiario> ResultadosDiario { get; set; }
        public DbSet<ResultadoIndicador> ResultadoIndicador { get; set; }
        public DbSet<ResultadoIndicadorSgi> ResultadosIndicadorSgi { get; set; }
        public DbSet<Sgi> Sgis { get; set; }
        public DbSet<Indicador> TpsIndicador { get; set; }
        public DbSet<Recurso> TpsRecurso { get; set; }
        public DbSet<TpRelatorio> TpsRelatorio { get; set; }
        public DbSet<TpResultadoContestacao> TpsResultadoContestacao { get; set; }
        public DbSet<UtrCd> UtrCds { get; set; }
        public DbSet<Cos> Cos { get; set; }
        public DbSet<ValorParametroSistema> ValorParametroSistema { get; set; }
        public DbSet<TpIndicador> TpIndicador { get; set; }
        public DbSet<LogEvento> LogEvento { get; set; }

        // SQl View
        public DbSet<AgenteIndicadorView> AgenteIndicadorView { get; set; }
        public DbSet<ResultadoIndicadorView> ResultadoIndicadorView { get; set; }
        public DbSet<InstalacaoView> InstalacaoView { get; set; }
        public DbSet<RecursoView> RecursoView { get; set; }
        public DbSet<ContestacaoAgenteView> ContestacaoAgenteView { get; set; }
        public DbSet<ConsultaIndicadorInstalacaoRecusoView> ConsultaIndicadorInstalacaoRecusoView { get; set; }
        public DbSet<ExportarInstalacaoView> ExportarInstalacaoView { get; set; }
        public DbSet<ScadaView> ScadaView { get; set; }
        public DbSet<SupervisaoTempoRealView> SupervisaoTempoRealView { get; set; }
        public DbSet<ContestacaoInstalacaoRecursoView> ContestacaoInstalacaoRecursoView { get; set; }
        public DbSet<ResultadoDiarioDCDView> ResultadoDiarioDCDView { get; set; }
        public DbSet<DadosInstalacaoView> DadosInstalacaoView { get; set; }
        public DbSet<InstalacaoConsultarMedidaView> InstalacaoConsultarMedidaView { get; set; }
        public DbSet<SgiView> SgiView { get; set; }
        public DbSet<ResultadoDiarioDRSCView> ResultadoDiarioDRSCView { get; set; }
        public DbSet<CalendarioDataExisteView> CalendarioDataExisteView { get; set; }
        public DbSet<InstalacaoRecursoRelatorioAgenteView> InstalacaoRecursoRelatorioAgenteView { get; set; }
        public DbSet<IndicadorInstalacaoRelatorioAgenteView> IndicadorInstalacaoRelatorioAgenteView { get; set; }

        #region ConsultaIndicador
        public DbSet<ConsultaIndicadorAgenteView> ConsultaIndicadorAgenteView { get; set; }
        public DbSet<ConsultaIndicadorSSCLView> ConsultaIndicadorSSCLView { get; set; }
        public DbSet<ConsultaIndicadorInstalacaoView> ConsultaIndicadorInstalacaoView { get; set; }
        #endregion

        public PortalMQDIDbContext(DbContextOptions<PortalMQDIDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region Debug
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif 
            #endregion

            optionsBuilder.UseLazyLoadingProxies(false);

            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
