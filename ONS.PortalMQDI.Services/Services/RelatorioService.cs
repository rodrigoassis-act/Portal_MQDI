using System;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using System.Threading;
using System.Collections.Generic;
using ONS.PortalMQDI.Data.Entity;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Shared.Utils;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Data.Entity.View;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Data.Repositories.Single;

namespace ONS.PortalMQDI.Services.Services
{
    public class RelatorioService : IRelatorioService
    {
        #region RelatorioService
        private readonly IAwsService _awsService;
        private readonly CargaRepository _cargaRepository;
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatorioService(
            IAwsService awsService,
            CargaRepository cargaRepository,
            IRelatorioRepository relatorioRepository
            )
        {
            _awsService = awsService;
            _cargaRepository = cargaRepository;
            _relatorioRepository = relatorioRepository;
        }
        #endregion

        #region Relatorio Mensal Consolidado de Acompanhamento de Qualidade e Disponibilidade
        public void GerarRMCAcompanhamentoQualidade(AutomacaoIndicador automacao)
        {
            var indicadores = automacao.EntryIndicadores.Cast<TpIndicador>().ToList();

            foreach (var indicador in indicadores)
            {
                StringBuilder tabelaAgente = new StringBuilder();
                StringBuilder tabelaSSCL = new StringBuilder();
                StringBuilder tabelaInstalacao = new StringBuilder();
                StringBuilder htmlTemp = new StringBuilder();

                var templeteBody = new TempleteUtil(TempleteEnum.AutomacaoIndicador.GetDescription());

                var entryAgentes = automacao.EntryAgentes.Cast<ConsultaIndicadorAgenteView>()
                    .Where(c => c.CodIndicador == indicador.CodIndicador)
                    .ToList();

                var entryResultadoIndicador = automacao.EntryResultadoIndicador.Cast<ConsultaIndicadorSSCLView>()
                    .Where(c => c.CodIndicador == indicador.CodIndicador).ToList();

                var entryInstalacao = automacao.EntryInstalacao.Cast<ExportarInstalacaoView>()
                     .Where(c => c.TipoIndicador == indicador.CodIndicador)
                     .OrderBy(c => c.NomeCurtoInstalacao)
                     .GroupBy(c => c.IdResultadoIndicador).Select(c => new
                     {
                         Instalacao = c.FirstOrDefault(),
                         Recurso = c.ToList()
                     }).ToList();

                if (entryAgentes.Count > 0)
                {
                    tabelaAgente.Append(MontarColunaAgente());
                }

                if (entryResultadoIndicador.Count > 0)
                {
                    tabelaSSCL.Append(MontarColunaSSCL());
                }

                foreach (var item in entryAgentes)
                {
                    var linha = new List<(string, bool)>
                            {
                                (item.NomeCurto, false ),
                                (item.ValAnual.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoAnual)),
                                (item.ValMensal.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoMensal))
                            };

                    tabelaAgente.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                }

                foreach (var item in entryResultadoIndicador)
                {
                    Enum.TryParse(item.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);

                    var linha = new List<(string, bool)>
                            {
                                (item.UtrCd, false ),
                                (cosIdEnum.GetDescription(), false),
                                (item.ValAnual.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoAnual)),
                                (item.ValMensal.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoMensal))
                            };

                    tabelaSSCL.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                }

                foreach (var item in entryInstalacao)
                {
                    StringBuilder tabelaInstalacaoTemp = new StringBuilder();
                    StringBuilder tabelaInstalacaoRecusoTemp = new StringBuilder();

                    if (item.Instalacao != null)
                    {
                        tabelaInstalacaoTemp.Append(MontarColunaInstalacao());

                        Enum.TryParse(item.Instalacao.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);

                        var linha = new List<(string, bool)>
                            {
                                (item.Instalacao.NomeCurtoInstalacao, false ),
                                (cosIdEnum.GetDescription(), false),
                                (item.Instalacao.ValorInstalacaoAnual.Value.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.Instalacao.InstalacaoFragAnual)),
                                (item.Instalacao.ValorInstalacaoMensal.Value.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.Instalacao.InstalacaoFragMensal))
                            };

                        tabelaInstalacaoTemp.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                    }

                    if (item.Recurso.Count() > 1)
                    {
                        var recursoTemp = item.Recurso.Where(c => !c.RecursoFragAnual.Value || !c.RecursoFragMensal.Value).OrderBy(c => c.Grandeza).ToList();

                        if (recursoTemp.Count() > 0)
                        {
                            tabelaInstalacaoRecusoTemp.Append(MontarColunaInstalacaoRecuso());
                        }

                        foreach (var recurso in recursoTemp)
                        {
                            Enum.TryParse(recurso.CodRede, out TipoRedeEnum tipoRedeEnum);
                            Enum.TryParse(recurso.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);
                            var linhaRecurso = new List<(string, bool)>
                            {
                                (recurso.Grandeza, false),
                                (cosIdEnum.GetDescription(), false),
                                (recurso.DescricaoGrandeza, false),
                                (recurso.ValorRecursoAnual.Value.RoundToTwoDecimalPlaces().ToString(), PortalMQDIUtil.Violacao(recurso.RecursoFragAnual.Value)),
                                (recurso.ValorRecursoMensal.Value.RoundToTwoDecimalPlaces().ToString(), PortalMQDIUtil.Violacao(recurso.RecursoFragMensal.Value)),
                                (recurso.NomeCurtoInstalacao, false),
                                (tipoRedeEnum.GetDescription(), false),
                                (recurso.Lscinf, false),
                                (recurso.NomEnderecoFisico, false)
                            };

                            tabelaInstalacaoRecusoTemp.AppendLine(HTMLUtil.GenerateTableBodyRows(linhaRecurso));
                        }
                    }

                    tabelaInstalacao.AppendLine(HTMLUtil.GenerateTable(tabelaInstalacaoTemp.ToString()));
                    tabelaInstalacao.AppendLine(HTMLUtil.GenerateTable(tabelaInstalacaoRecusoTemp.ToString()));
                }



                if (tabelaAgente.Length > 0)
                {
                    htmlTemp.AppendLine(HTMLUtil.GenerateTable(tabelaAgente.ToString()));
                }

                if (tabelaSSCL.Length > 0)
                {
                    htmlTemp.AppendLine(HTMLUtil.GenerateTable(tabelaSSCL.ToString()));
                }

                if (tabelaInstalacao.Length > 0)
                {
                    htmlTemp.AppendLine(tabelaInstalacao.ToString());
                }

                var estilos = templeteBody.ReadHtmlFile("styleTemplete.html");
                var templete = templeteBody.ReadHtmlFile("templete.html")
                    .Replace("{{tabela}}", htmlTemp.ToString())
                    .Replace("{{styleTemplete}}", estilos);

                var headerCustom = templeteBody.ReadHtmlFile("pageHead.html")
                   .Replace("{{anoMes}}", automacao.AnoMes.ConvertAnomeReferenciaToDate())
                   .Replace("{{agente}}", automacao.NomeAgente)
                   .Replace("{{indicador}}", indicador.CodIndicador);

                var byteArray = DocumentGeneratorUtil
                    .GeneratePdfFromHtml(
                    templete,
                    "Relatorio Mensal Consolidado de Acompanhamento de Qualidade e Disponibilidade (RAcDQ)",
                    headerCustom);


                var nomeArquivo = $"{automacao.Arquivo.NomeArquivo.Replace("!indicador!", indicador.CodIndicador)}.pdf";

                var uploadFile = _awsService.UploadFileAsync(automacao.Arquivo.PastaArquivo, byteArray, nomeArquivo, CancellationToken.None).GetAwaiter().GetResult();

                if (!String.IsNullOrEmpty(uploadFile))
                {
                    RegistraRelatorio(automacao.AnoMes, indicador.Id, automacao.TipoRelatorio, automacao.IdAgente);
                }
            }
        }
        #endregion

        #region Relatorio Mensal Consolidado de Violações de Qualidade e Disponibilidade
        public async void GerarRMCViolacoesQualidade(AutomacaoIndicador automacao)
        {
            var indicadores = automacao.EntryIndicadores.Cast<TpIndicador>().ToList();

            foreach (var indicador in indicadores)
            {
                StringBuilder tabelaAgente = new StringBuilder();
                StringBuilder tabelaSSCL = new StringBuilder();
                StringBuilder tabelaInstalacao = new StringBuilder();
                StringBuilder htmlTemp = new StringBuilder();

                var templeteBody = new TempleteUtil(TempleteEnum.AutomacaoIndicador.GetDescription());

                var entryAgentes = automacao.EntryAgentes.Cast<ConsultaIndicadorAgenteView>()
                    .Where(c => c.CodIndicador == indicador.CodIndicador && (!c.FlgViolacaoAnual || !c.FlgViolacaoMensal))
                    .ToList();

                var entryResultadoIndicador = automacao.EntryResultadoIndicador.Cast<ConsultaIndicadorSSCLView>()
                    .Where(c => c.CodIndicador == indicador.CodIndicador && (!c.FlgViolacaoAnual || !c.FlgViolacaoMensal)).ToList();

                var entryInstalacao = automacao.EntryInstalacao.Cast<ExportarInstalacaoView>()
                     .Where(c => c.TipoIndicador == indicador.CodIndicador)
                     .OrderBy(c => c.NomeCurtoInstalacao)
                     .GroupBy(c => c.IdResultadoIndicador).Select(c => new
                     {
                         Instalacao = c.FirstOrDefault(x => !x.InstalacaoFragAnual || !x.InstalacaoFragMensal),
                         Recurso = c.ToList()
                     }).ToList();

                if (entryAgentes.Count > 0)
                {
                    tabelaAgente.Append(MontarColunaAgente());
                }

                if (entryResultadoIndicador.Count > 0)
                {
                    tabelaSSCL.Append(MontarColunaSSCL());
                }

                foreach (var item in entryAgentes)
                {
                    var linha = new List<(string, bool)>
                            {
                                (item.NomeCurto, false ),
                                (item.ValAnual.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoAnual)),
                                (item.ValMensal.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoMensal))
                            };

                    tabelaAgente.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                }

                foreach (var item in entryResultadoIndicador)
                {
                    Enum.TryParse(item.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);

                    var linha = new List<(string, bool)>
                            {
                                (item.UtrCd, false ),
                                (cosIdEnum.GetDescription(), false),
                                (item.ValAnual.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoAnual)),
                                (item.ValMensal.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.FlgViolacaoMensal))
                            };

                    tabelaSSCL.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                }

                foreach (var item in entryInstalacao)
                {
                    StringBuilder tabelaInstalacaoTemp = new StringBuilder();
                    StringBuilder tabelaInstalacaoRecusoTemp = new StringBuilder();

                    if (item.Instalacao != null)
                    {
                        tabelaInstalacaoTemp.Append(MontarColunaInstalacao());

                        Enum.TryParse(item.Instalacao.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);

                        var linha = new List<(string, bool)>
                            {
                                (item.Instalacao.NomeCurtoInstalacao, false ),
                                (cosIdEnum.GetDescription(), false),
                                (item.Instalacao.ValorInstalacaoAnual.Value.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.Instalacao.InstalacaoFragAnual)),
                                (item.Instalacao.ValorInstalacaoMensal.Value.RoundToTwoDecimalPlaces(), PortalMQDIUtil.Violacao(item.Instalacao.InstalacaoFragMensal))
                            };

                        tabelaInstalacaoTemp.AppendLine(HTMLUtil.GenerateTableBodyRows(linha));
                    }

                    if (item.Recurso.Count() > 1)
                    {
                        var recursoTemp = item.Recurso.Where(c => !c.RecursoFragAnual.Value || !c.RecursoFragMensal.Value).OrderBy(c => c.Grandeza).ToList();

                        if (recursoTemp.Count() > 0)
                        {
                            tabelaInstalacaoRecusoTemp.Append(MontarColunaInstalacaoRecuso());
                        }

                        foreach (var recurso in recursoTemp)
                        {
                            Enum.TryParse(recurso.CodRede, out TipoRedeEnum tipoRedeEnum);
                            Enum.TryParse(recurso.CosId.Replace("COSR-", ""), out CentroOperacaoEnum cosIdEnum);
                            var linhaRecurso = new List<(string, bool)>
                            {
                                (recurso.Grandeza, false),
                                (cosIdEnum.GetDescription(), false),
                                (recurso.DescricaoGrandeza, false),
                                (recurso.ValorRecursoAnual.Value.RoundToTwoDecimalPlaces().ToString(), PortalMQDIUtil.Violacao(recurso.RecursoFragAnual.Value)),
                                (recurso.ValorRecursoMensal.Value.RoundToTwoDecimalPlaces().ToString(), PortalMQDIUtil.Violacao(recurso.RecursoFragMensal.Value)),
                                (recurso.NomeCurtoInstalacao, false),
                                (tipoRedeEnum.GetDescription(), false),
                                (recurso.Lscinf, false),
                                (recurso.NomEnderecoFisico, false)
                            };

                            tabelaInstalacaoRecusoTemp.AppendLine(HTMLUtil.GenerateTableBodyRows(linhaRecurso));
                        }
                    }

                    tabelaInstalacao.AppendLine(HTMLUtil.GenerateTable(tabelaInstalacaoTemp.ToString()));
                    tabelaInstalacao.AppendLine(HTMLUtil.GenerateTable(tabelaInstalacaoRecusoTemp.ToString()));
                }



                if (tabelaAgente.Length > 0)
                {
                    htmlTemp.AppendLine(HTMLUtil.GenerateTable(tabelaAgente.ToString()));
                }

                if (tabelaSSCL.Length > 0)
                {
                    htmlTemp.AppendLine(HTMLUtil.GenerateTable(tabelaSSCL.ToString()));
                }

                if (tabelaInstalacao.Length > 0)
                {
                    htmlTemp.AppendLine(tabelaInstalacao.ToString());
                }

                var estilos = templeteBody.ReadHtmlFile("styleTemplete.html");
                var templete = templeteBody.ReadHtmlFile("templete.html")
                    .Replace("{{tabela}}", htmlTemp.ToString())
                    .Replace("{{styleTemplete}}", estilos);

                var headerCustom = templeteBody.ReadHtmlFile("pageHead.html")
                    .Replace("{{anoMes}}", automacao.AnoMes.ConvertAnomeReferenciaToDate())
                    .Replace("{{agente}}", automacao.NomeAgente)
                    .Replace("{{indicador}}", indicador.CodIndicador);

                var byteArray = DocumentGeneratorUtil
                    .GeneratePdfFromHtml(
                    templete,
                    "Relatório Mensal Consolidado de Acompanhamento dos Indicadores de Violações de Disponibilidade (RAvDQ)",
                    headerCustom);

                var nomeArquivo = $"{automacao.Arquivo.NomeArquivo.Replace("!indicador!", indicador.CodIndicador)}.pdf";


                var uploadFile = _awsService.UploadFileAsync(automacao.Arquivo.PastaArquivo, byteArray, nomeArquivo, CancellationToken.None).GetAwaiter().GetResult();

                if (!String.IsNullOrEmpty(uploadFile))
                {
                    RegistraRelatorio(automacao.AnoMes, indicador.Id, automacao.TipoRelatorio, automacao.IdAgente);
                }
            }
        }

        #endregion

        #region Relatório Mensal de Indicadores de Qualidade e Disponibilidade
        public void GerarRMIndicadoresQualidade(AutomacaoIndicador automacao)
        {
            var entryMedidas = automacao.EntryMedidas;
            var entryAgentes = automacao.EntryAgentes.Cast<ConsultaIndicadorAgenteView>().ToList();
            var entryResultadoIndicador = automacao.EntryResultadoIndicador.Cast<ConsultaIndicadorSSCLView>().ToList();
            var entryInstalacao = automacao.EntryInstalacao.Cast<InstalacaoRecursoRelatorioAgenteView>().ToList();
            var arquivo = automacao.Arquivo;

            var agruparTipoRecurso = entryInstalacao.GroupBy(i => i.TipoIndicador).Select(c => c.Key);

            string nomeArquivo = string.Empty;
            using (var workbook = new XLWorkbook())
            {
                #region Agente
                var agenteWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, "Agente", new List<string>
                    {
                        "Agente", "Indicador", "Violação Anual", "Anual (%)", "Violação Mensal", "Mensal (%)"
                    });

                var dadosAgente = new List<string[]>();

                foreach (var itemAgente in entryAgentes)
                {
                    dadosAgente.Add(new string[] {
                        itemAgente.NomeLongo,
                        itemAgente.CodIndicador,
                        itemAgente.FlgViolacaoAnual ? "Não" : "Sim",
                        itemAgente.ValAnual.RoundToTwoDecimalPlaces(),
                        itemAgente.FlgViolacaoMensal ? "Não" : "Sim",
                        itemAgente.ValMensal.RoundToTwoDecimalPlaces(),
                    });
                }


                ExcelUtil.FillWorksheetData(agenteWorkshee, dadosAgente);
                #endregion

                #region SSCL
                var ssclWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, "SSCL", new List<string>
                    {
                        "Centro", "SSCL", "Indicador", "Violação Anual", "Anual (%)", "Violação Mensal", "Mensal (%)"
                    });

                var dadosSSCL = new List<string[]>();

                foreach (var itemSSLC in entryResultadoIndicador)
                {
                    var cosIdWithoutPrefix = itemSSLC.CosId.Replace("COSR-", "");
                    Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);
                    dadosSSCL.Add(new string[] {
                        cosIdEnum.GetDescription(),
                        itemSSLC.UtrCd,
                        itemSSLC.CodIndicador,
                        itemSSLC.FlgViolacaoAnual ? "Não" : "Sim",
                        itemSSLC.ValAnual.RoundToTwoDecimalPlaces(),
                        itemSSLC.FlgViolacaoMensal ? "Não" : "Sim",
                       itemSSLC.ValMensal.RoundToTwoDecimalPlaces(),
                    });
                }


                ExcelUtil.FillWorksheetData(ssclWorkshee, dadosSSCL);
                #endregion

                #region Instalacão
                foreach (var tipoRecurso in agruparTipoRecurso)
                {
                    var headers = new List<string>
                    {
                        "Cod.Grandeza",
                        "Descr.Grandeza",
                        "Violação Anual",
                        "Anual (%)",
                        "Violação Mensal",
                        "Mensal (%)",
                        "Instalação",
                        "Rede",
                        "Ligação SSC",
                        "Endereço do Protocolo"
                    };

                    var instalacao = ExcelUtil.CreateWorksheetWithHeaders(workbook, tipoRecurso, headers);

                    var agruparInstalacao = entryInstalacao.Where(c => c.TipoIndicador == tipoRecurso)
                        .GroupBy(c => c.NomeCurto).Select(c => new
                        {
                            NomeCurto = c.Key,
                            Instalacao = c.ToList()
                        });

                    var dadosInstalacao = new List<string[]>();

                    foreach (var instalacaoItem in agruparInstalacao)
                    {
                        for (int c = 0; c < instalacaoItem.Instalacao.Count(); c++)
                        {
                            var item = instalacaoItem.Instalacao[c];

                            Enum.TryParse(item.CodRede, out TipoRedeEnum tipoRedeEnum);

                            dadosInstalacao.Add(new string[] {
                                item.Grandeza,
                                item.DescricaoGrandeza,
                                item.ViolacaoAnual ? "Não" : "Sim",
                                TratamentoValor(item.ValorAnual),
                                item.ViolacaoMensal ? "Não" : "Sim",
                                TratamentoValor(item.ValorMensal),
                                item.NomeCurto,
                                tipoRedeEnum.GetDescription(),
                                item.Lscinf,
                                item.EnderecoFisico
                            });
                        }

                        //Cria a linha amarela da instalação no relatório
                        var itemDestaque = instalacaoItem.Instalacao.FirstOrDefault();
                        var destaqueInstalacao = _cargaRepository.ObterIndicadorInstalacao(itemDestaque.AnoMesReferencia.ConvertToAnomeReferencia(), itemDestaque.AgeMrid, itemDestaque.InsMrid, itemDestaque.IdTipoIndicador);

                        var destaque = new string[] {
                                "******",
                                destaqueInstalacao.NomeCurto,
                                "",
                                destaqueInstalacao.ValorAnual.Value.RoundToTwoDecimalPlaces(),
                                "",
                                destaqueInstalacao.ValorMensal.Value.RoundToTwoDecimalPlaces(),
                                "",
                                "",
                                "",
                                ""
                        };
                        dadosInstalacao.Add(destaque);
                        ExcelUtil.FillHighlightedRow(instalacao, (dadosInstalacao.Count + 1), destaque, XLColor.Yellow);

                        ExcelUtil.FillWorksheetData(instalacao, dadosInstalacao);
                    }
                }

                #endregion

                #region Medidas Não-Supervisionadas 
                var medidasWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, "Medidas Não Supervisionadas", new List<string>
                {
                    "Nome Instalação", "Identificador ONS", "Código Instalação", "Centro", "Endereço do Protocolo", "Descrição", "Rede"
                });

                var dadosMedidas = new List<string[]>();

                foreach (var itemMedida in entryMedidas)
                {
                    foreach (var itemRecurso in itemMedida.Recurso)
                    {
                        dadosMedidas.Add(new string[] {
                            itemMedida.NomeInstalacao,
                            itemRecurso.IdoOns,
                            itemMedida.IdInstalacao,
                            itemRecurso.CosId,
                            itemRecurso.IdoOns,
                            itemRecurso.DscGrandeza,
                            itemRecurso.TpRede
                        });
                    }
                }

                ExcelUtil.FillWorksheetData(medidasWorkshee, dadosMedidas);
                #endregion

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                byte[] byteArray = ms.ToArray();

                var uploadFile = _awsService.UploadFileAsync(automacao.Arquivo.PastaArquivo, byteArray, $"{arquivo.NomeArquivo}.xlsx", CancellationToken.None).GetAwaiter().GetResult();

                if (!String.IsNullOrEmpty(uploadFile))
                {
                    RegistraRelatorio(automacao.AnoMes, 0, automacao.TipoRelatorio, automacao.IdAgente);
                }
            }
        }
        #endregion

        #region Relatório Mensal de Manutenção de Disponibildade
        public void GerarRMManutençãoDisponibildade(List<DadosInstalacaoView> grandezaEntry, string anoMes)
        {
            var groupGrandeza = grandezaEntry.GroupBy(c => c.IdPonto).Select(c => new
            {
                Grandeza = c.FirstOrDefault(),
                Itens = c.ToList()
            });

            using (var workbook = new XLWorkbook())
            {
                var grandezaWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, nameof(TipoRelatorioEnum.RAmD), new List<string>
                {
                    "Agente",
                    "Cód. agente",
                    "Centro Regional ",
                    "Violação Anual",
                    "Anual (%)",
                    "Instalação ",
                    "Cód. Ins",
                    "ID do ponto",
                    "Expurgo Estado Operativo",
                    "Expurgo Historico PI",
                    "UTRCD",
                    "Endereço do Protocolo",
                    "Índice da última apuração",
                    "Índice da penúltima apuração",
                    "Índice da antepenúltima apuração"
                });
                var dadosGrandeza = new List<string[]>();

                foreach (var item in groupGrandeza)
                {
                    var cosIdWithoutPrefix = item.Grandeza.CentroOperacao.Replace("COSR-", "");
                    Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);

                    var indiceUltimaApuracao = PegarDisponibilidadePorIndice(item.Itens, 0, anoMes);
                    var indicePenultimaApuracao = PegarDisponibilidadePorIndice(item.Itens, 1, anoMes);
                    var indiceAntepenultimaApuracao = PegarDisponibilidadePorIndice(item.Itens, 2, anoMes);

                    dadosGrandeza.Add(new string[] {
                        item.Grandeza.NomeAgente,
                        item.Grandeza.IdAgente,
                        cosIdEnum.GetDescription(),
                        item.Grandeza.FlagViolacaoAnual ? "Não" : "Sim",
                        TratamentoValor(item.Grandeza.IndiceAnual),
                        item.Grandeza.NomeInstalacao,
                        item.Grandeza.IdInstalacao,
                        item.Grandeza.IdPonto,
                        item.Grandeza.ExpurgoEstadoOperativo,
                        item.Grandeza.ExpurgoHistoricoPI,
                        item.Grandeza.UTRCD,
                        item.Grandeza.Endereco,
                        indiceUltimaApuracao,
                        indicePenultimaApuracao,
                        indiceAntepenultimaApuracao
                    });
                }

                ExcelUtil.FillWorksheetData(grandezaWorkshee, dadosGrandeza);

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                byte[] byteArray = ms.ToArray();

                var uploadFile = _awsService.UploadFileAsync(nameof(TipoRelatorioEnum.RAmD), byteArray, $"{nameof(TipoRelatorioEnum.RAmD)}_{anoMes.Replace("-", "_")}.xlsx", CancellationToken.None).GetAwaiter().GetResult();

                if (!String.IsNullOrEmpty(uploadFile))
                {
                    RegistraRelatorio(anoMes, 0, (int)TipoRelatorioEnum.RAmD, "ONS");
                }
            }
        }
        #endregion

        #region Auxiliares
        private void RegistraRelatorio(string anoMes, int idIndicador, int idTpRelatorio, string agendeId)
        {
            if (_relatorioRepository.ExistsAsync(c => c.AnomesReferencia == anoMes
            && c.AgenteId == agendeId
            && c.IdTpIndicador == idIndicador
            && c.IdTpRelatorio == idTpRelatorio, CancellationToken.None).GetAwaiter().GetResult() == false)
            {
                var relatorio = new Relatorio
                {
                    IdTpIndicador = idIndicador,
                    AgenteId = agendeId,
                    AnomesReferencia = anoMes,
                    IdTpRelatorio = idTpRelatorio,
                    GeracaoRelatorio = DateTime.Now
                };

                _relatorioRepository.InsertAsync(relatorio, CancellationToken.None).GetAwaiter().GetResult();
            }
        }

        private string PegarDisponibilidadePorIndice(List<DadosInstalacaoView> itens, int indice, string dataAtual)
        {
            var rangeDatas = dataAtual.GeneratePastMonths(3);
            var datas = rangeDatas.Select(data => data.ToString("yyyy-MM")).OrderByDescending(c => c).ToList();

            var item = itens.OrderByDescending(c => c.AnoMesReferencia).FirstOrDefault(c => c.AnoMesReferencia == datas[indice]);

            if (item != null)
            {
                return item.Disponibilidade.RoundToTwoDecimalPlaces();
            }
            else
            {
                return "0.0";
            }
        }

        private string MontarColunaAgente()
        {
            var coluna = new string[] { "Agente", "DRSC (%) Anual", "DRSC (%) Mensal" };
            return HTMLUtil.GenerateTableHeaderRow(coluna);
        }

        private string MontarColunaSSCL()
        {
            var coluna = new string[] { "SSCL/CD", "Centro", "DRSC (%) Anual", "DRSC (%) Mensal" };
            return HTMLUtil.GenerateTableHeaderRow(coluna);
        }

        private string MontarColunaInstalacao()
        {
            var coluna = new string[] { "Instalação", "Centro", "DRSC (%) Anual", "DRSC (%) Mensal" };
            return HTMLUtil.GenerateTableHeaderRow(coluna);
        }

        private string MontarColunaInstalacaoRecuso()
        {
            var coluna = new string[] { "Identificador ONS", "Centro", "Descrição", "DRSC (%) Anual", "DRSC (%) Mensal", "Instalação", "Rede", "Ligação SSC", "Endereço de Protocolo" };
            return HTMLUtil.GenerateTableHeaderRow(coluna);
        }

        public string TratamentoValor(double? valor)
        {
            if (valor.HasValue)
            {
                return valor.Value.RoundToTwoDecimalPlaces();
            }
            return null;
        }
        #endregion
    }
}
