using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ONS.PortalMQDI.Services.Services
{
    public class CalendarioCalculator : ICalendarioCalculator
    {
        private readonly IValorParametroSistemaRepository _valorParametroSistemaRepository;
        private readonly IFeriadoRepository _feriadoRepository;

        public CalendarioCalculator(
            IValorParametroSistemaRepository valorParametroSistemaRepository,
            IFeriadoRepository feriadoRepository)
        {
            _valorParametroSistemaRepository = valorParametroSistemaRepository;
            _feriadoRepository = feriadoRepository;
        }

        public bool VerificarSeCalendarioEstaValido(CalendarioSistema calendario)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1) >= calendario.DataTerminoPeriodoAnaliseContestacoes;
        }

        public CalendarioSistema CalculaCalendarioDoSistema()
        {
            var calendario = new CalendarioSistema();
            var parametros = _valorParametroSistemaRepository.RetornaParametrosBy(p => p.DataFimVigencia == null).ToDictionary(p => p.ParametroSistema.NomeParametro, p => p);
            var feriadosNoMes = _feriadoRepository.ListaFeriadosNoMes(DateTime.Now.ToString("yyyy-MM")).ToList();
            var valorParametroDiaUtilOuCorrido = _valorParametroSistemaRepository.RetornaValorAtualDoParametroPorNome(ApplicationConstants.ParaMetroDiaUtilOuCorrido).ValParametro;

            DateTime dataLiberacao, dataTerminoContestacao, dataTerminoPeriodoAnaliseONS;

            if (valorParametroDiaUtilOuCorrido == ApplicationConstants.ValorParametroParaDiaUtil)
            {
                dataLiberacao = BuscaDataDosParametros(parametros[ApplicationConstants.ParametroLiberacaoAgente], feriadosNoMes);
                dataTerminoContestacao = BuscaDataDosParametros(parametros[ApplicationConstants.ParametroTerminoPeriodoContestacao], feriadosNoMes, dataLiberacao);
                dataTerminoPeriodoAnaliseONS = BuscaDataDosParametros(parametros[ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao], feriadosNoMes, dataTerminoContestacao);
            }
            else
            {
                dataLiberacao = CalculaDataCorrida(parametros[ApplicationConstants.ParametroLiberacaoAgente]);
                dataTerminoContestacao = CalculaDataCorrida(parametros[ApplicationConstants.ParametroTerminoPeriodoContestacao], dataLiberacao);
                dataTerminoPeriodoAnaliseONS = CalculaDataCorrida(parametros[ApplicationConstants.ParametroTerminoPeriodoAnaliseContestacao], dataTerminoContestacao);
            }

            calendario.DataLiberacaoContestacoesAgente = dataLiberacao;
            calendario.DataTerminoPeriodoContestacoesAgente = dataTerminoContestacao;
            calendario.DataTerminoPeriodoAnaliseContestacoes = dataTerminoPeriodoAnaliseONS;

            return calendario;
        }

        private DateTime BuscaDataDosParametros(ValorParametroSistema parametro, List<Feriado> feriadosNoMes, DateTime? dataInicial = null)
        {
            var valorDoParametro = int.Parse(parametro.ValParametro);
            DateTime dataDoParametro, startDate;

            if (dataInicial != null)
            {
                dataDoParametro = dataInicial.Value.AddDays(1);
                startDate = dataInicial.Value.AddDays(1);
            }
            else
            {
                dataDoParametro = DateTime.Today.AddDays(1);
                startDate = DateTime.Today.AddDays(1);
            }

            var endDate = startDate.AddMonths(2).AddDays(-1);

            while (valorDoParametro > 0)
            {
                if (startDate > endDate)
                {
                    throw new Exception("Parâmetros cadastrados estão ultrapassando o limite de 2 meses");
                }
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (VerificaSeDataEDiaUtil(date, feriadosNoMes))
                    {
                        valorDoParametro--;
                    }
                    if (valorDoParametro == 0)
                    {
                        break;
                    }
                    else
                    {
                        dataDoParametro = dataDoParametro.AddDays(1);
                    }
                }
            }

            return dataDoParametro;
        }

        private DateTime CalculaDataCorrida(ValorParametroSistema parametro, DateTime? dataInicial = null)
        {
            DateTime dataDoParametro = dataInicial ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int valorDoParametro = int.Parse(parametro.ValParametro);
            dataDoParametro = dataDoParametro.AddDays(valorDoParametro - 1);

            return dataDoParametro;
        }

        private bool VerificaSeDataEDiaUtil(DateTime date, List<Feriado> listaDeFeriadosDoMes)
        {
            var cond1 = ((date.DayOfWeek >= DayOfWeek.Monday) && (date.DayOfWeek <= DayOfWeek.Friday));
            var cond2 = !listaDeFeriadosDoMes.Any(f => f.DataFeriado.Value.Date == date.Date);

            return cond1 && cond2;
        }
    }
}
