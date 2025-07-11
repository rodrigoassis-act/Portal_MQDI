using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.ViewModel;
using ONS.PortalMQDI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class ParametroSistemaService : IParametroSistemaService
    {
        private readonly IValorParametroSistemaRepository _valorParametroSistemaRepository;
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly ICalendarioSistemaRepository _calendarioSistemaRepository;
        private readonly ICalendarioService _calendarioService;
        public ParametroSistemaService(
            IValorParametroSistemaRepository valorParametroSistemaRepository,
            IResultadoIndicadorRepository resultadoIndicadorRepository,
            ICalendarioSistemaRepository calendarioSistemaRepository,
            ICalendarioService calendarioService)
        {
            _valorParametroSistemaRepository = valorParametroSistemaRepository;
            _resultadoIndicadorRepository = resultadoIndicadorRepository;
            _calendarioSistemaRepository = calendarioSistemaRepository;
            _calendarioService = calendarioService;
        }

        public async Task<List<ParametroSistemaViewModel>> BuscaValorLimiteDeIndicadoresAsync(CancellationToken cancellationToke)
        {
            var parametrosAtuais = await _valorParametroSistemaRepository.BuscarValorLimiteAsync(cancellationToke);

            return parametrosAtuais.Select(param => new ParametroSistemaViewModel
            {
                Id = param.IdParametroSistema,
                IdParametroSistema = param.ParametroSistema.IdParametro,
                NomParametro = param.ParametroSistema.NomeParametro,
                NomTpParametro = param.ParametroSistema.TipoParametro,
                ValParametro = param.ValParametro,
                NovoValParametro = ""
            }).OrderBy(param => param.IdParametroSistema).ToList();
        }

        public async Task<List<ParametroSistemaViewModel>> BuscaValorParametroSistemaAsync(CancellationToken cancellationToken)
        {
            var parametrosSistema = await _valorParametroSistemaRepository.BuscaValorParametroSistemaAsync(cancellationToken);
            return parametrosSistema.Select(p => new ParametroSistemaViewModel
            {
                Id = p.IdParametro,
                IdParametroSistema = p.IdParametroSistema,
                ValParametro = p.ValParametro,
                NomParametro = p.ParametroSistema.NomeParametro,
                NomTpParametro = p.ParametroSistema.TipoParametro,
                NovoValParametro = null,
            }).OrderBy(p => p.IdParametroSistema).ToList();
        }

        public async Task<ValorParametroSistema> AdicionarNovoParametroAsync(ParametroSistemaViewModel param, CancellationToken cancellationToken)
        {
            var valorParametroAntigo = await _valorParametroSistemaRepository.RetornaValorDeParametroAtualPorIdParametroAsync(param.Id, cancellationToken);

            param.NovoValParametro = MascaraNovoValParametro(param);

            valorParametroAntigo.DataFimVigencia = DateTime.Now;

            var valorParametroNovo = new ValorParametroSistema
            {
                IdParametro = 0,
                IdParametroSistema = valorParametroAntigo.IdParametroSistema,
                DataFimVigencia = null,
                ValParametro = param.NovoValParametro
            };

            await _valorParametroSistemaRepository.InsertAsync(valorParametroNovo, cancellationToken);
            await _valorParametroSistemaRepository.UpdateAsync(param.Id, valorParametroAntigo, cancellationToken);
            var save = await _valorParametroSistemaRepository.SaveChangesAsync(cancellationToken);

            return valorParametroNovo;
        }

        public async Task<bool> ExisteRegistroFaltanteNoCalendarioAsync(string anoMes, CancellationToken cancellationToken)
        {
            var maiorCalendarioRegistrado = await _calendarioSistemaRepository.BuscaDatasPorDataReferenciaAsync(anoMes, cancellationToken);

            if (maiorCalendarioRegistrado == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AdicionaRegistroFaltanteNoCalendarioAsync(string anoMes, CancellationToken cancellationToken)
        {
            string maiorDataReferenciaIndicadorExistente = _resultadoIndicadorRepository.BuscarDataReferenciaMaisRecente();

            if (ExisteRegistroFaltanteNoCalendarioAsync(anoMes, cancellationToken).Result)
            {
                if (!_calendarioService.GeraCalendarioDoSistema(anoMes.Replace('/', '-'), cancellationToken))
                {
                    throw new Exception("Parâmetros cadastrados estão ultrapassando o limite de 2 meses");
                }
            }
            return false;
        }

        private string MascaraNovoValParametro(ParametroSistemaViewModel param)
        {
            if (param.NovoValParametro.Contains('.'))
            {
                decimal novoVal = decimal.Parse(param.NovoValParametro.Replace(".", ","));
                byte decimals = (byte)((Decimal.GetBits(novoVal)[3] >> 16) & 0x7F);
                if (decimals < 2)
                {
                    return param.NovoValParametro += "0";
                }
            }
            else if (param.NomParametro.StartsWith("Valor limite"))
            {
                return param.NovoValParametro += ".00";
            }

            return param.NovoValParametro;
        }

        public async Task<int> LimpezaCompletaAsync(string anoMes, CancellationToken cancellationToken)
        {
            var result = await _valorParametroSistemaRepository.ExecutarQueryLimpezaCompletaAsync(anoMes, cancellationToken);
            return result;
        }
    }
}