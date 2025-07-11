using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Data.Interfaces;
using ONS.PortalMQDI.Models.Enum;
using ONS.PortalMQDI.Models.Model;
using ONS.PortalMQDI.Models.ViewModel.Filtros;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Constants;
using ONS.PortalMQDI.Shared.Extensions;
using ONS.PortalMQDI.Shared.Settings;
using ONS.PortalMQDI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ONS.PortalMQDI.Services.Services
{
    public class ExportarService : IExportarService
    {
        private readonly IOptions<ServiceGlobalSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResultadoIndicadorService _resultadoIndicadorService;
        private readonly IInstalacaoService _instalacaoService;
        private readonly IInstalacaoRepository _instalacaoRepository;
        private readonly IAgenteRepository _agenteRepository;
        private readonly IResultadoIndicadorRepository _resultadoIndicadorRepository;
        private readonly IAwsService _awsService;
        private readonly NotificacaoService _notificacaoService;
        private readonly IUserService _userService;
        private readonly IContestacaoRepository _contestacaoRepository;
        private readonly IGrandezaRepository _grandezaRepository;

        public ExportarService(IResultadoIndicadorService resultadoIndicadorService,
            IInstalacaoService instalacaoService,
            IInstalacaoRepository instalacaoRepository,
            IAgenteRepository agenteRepository,
            IResultadoIndicadorRepository resultadoIndicadorRepository,
            IAwsService awsService,
            NotificacaoService notificacaoService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ServiceGlobalSettings> settings,
            IContestacaoRepository contestacaoRepository,
            IGrandezaRepository grandezaRepository)
        {

            _resultadoIndicadorService = resultadoIndicadorService;
            _instalacaoService = instalacaoService;
            _instalacaoRepository = instalacaoRepository;
            _agenteRepository = agenteRepository;
            _resultadoIndicadorRepository = resultadoIndicadorRepository;
            _awsService = awsService;
            _notificacaoService = notificacaoService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
            _contestacaoRepository = contestacaoRepository;
            _grandezaRepository = grandezaRepository;
        }

        public FileContentResult ExportarConsultaindicadores(ExportarConsultaIndicadorViewModel filtro)
        {
            var entryAgentes = _agenteRepository
                .BuscarViewPorFiltro(filtro.MesAno, filtro.Agentes, filtro.Indicador, filtro.Violacao, false);

            var entryResultadoIndicador = _resultadoIndicadorRepository
                .BuscarViewPorFiltro(filtro.MesAno, filtro.Agentes, filtro.Indicador, filtro.Violacao, false);

            var entryInstalacao = _instalacaoRepository
                .ExportarInstalacao(filtro.Agentes, filtro.MesAno);

            var agruparTipoRecurso = entryInstalacao.GroupBy(i => i.TipoIndicador).Select(c => c.Key);

            string nomeArquivo = string.Empty;
            using (var workbook = new XLWorkbook())
            {

                #region Agente
                var agenteWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, "Agente", new List<string>
                {
                    "Agente", "Indicador", "Anual (%)", "Mensal (%)"
                });

                var dadosAgente = new List<string[]>();

                foreach (var itemAgente in entryAgentes)
                {
                    dadosAgente.Add(new string[] {
                        itemAgente.NomeLongo,
                        itemAgente.CodIndicador,
                        itemAgente.ValAnual.RoundToTwoDecimalPlaces(),
                        itemAgente.ValMensal.RoundToTwoDecimalPlaces(),
                    });
                }


                ExcelUtil.FillWorksheetData(agenteWorkshee, dadosAgente);
                #endregion

                #region SSCL
                var ssclWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, "SSCL", new List<string>
                {
                    "Centro", "SSCL", "Indicador", "Anual (%)", "Mensal (%)"
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
                        itemSSLC.ValAnual.RoundToTwoDecimalPlaces(),
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
                        "Anual (%)",
                        "Mensal (%)",
                        "Instalação",
                        "Rede",
                        "Ligação SSC",
                        "Endereço do Protocolo"
                    };

                    var instalacao = ExcelUtil.CreateWorksheetWithHeaders(workbook, tipoRecurso, headers);

                    var agruparInstalacao = entryInstalacao.Where(c => c.TipoIndicador == tipoRecurso)
                        .GroupBy(c => c.NomeCurtoInstalacao).Select(c => new
                        {
                            NomeCurtoInstalacao = c.Key,
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
                                item.ValorInstalacaoAnual.Value.RoundToTwoDecimalPlaces(),
                                item.ValorRecursoMensal.Value.RoundToTwoDecimalPlaces(),
                                item.NomeCurtoInstalacao,
                                tipoRedeEnum.GetDescription(),
                                item.Lscinf,
                                item.NomEnderecoFisico
                            });

                            if ((instalacaoItem.Instalacao.Count() - 1) == c)
                            {
                                var destaque = new string[] {
                                    "******",
                                    item.NomeCurtoInstalacao,
                                    item.ValorInstalacaoAnual.Value.RoundToTwoDecimalPlaces(),
                                    item.ValorInstalacaoMensal.Value.RoundToTwoDecimalPlaces(),
                                    "",
                                    "",
                                    "",
                                    ""
                                };

                                dadosInstalacao.Add(destaque);
                                ExcelUtil.FillHighlightedRow(instalacao, (dadosInstalacao.Count + 1), destaque, XLColor.Yellow);
                            }
                        }

                        ExcelUtil.FillWorksheetData(instalacao, dadosInstalacao);
                    }
                }

                #endregion

                nomeArquivo = $"{filtro.MesAno}.{DateTime.Now.ToString("ddMMyyyyHHmm")}.xlsx";

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                byte[] byteArray = ms.ToArray();

                var uploadFile = _awsService.UploadFileAsync("Arquivos_Temporarios", byteArray, nomeArquivo, CancellationToken.None).GetAwaiter().GetResult();

                if (!String.IsNullOrEmpty(uploadFile))
                {

                    string linkDownload = $"{_settings.Value.PortalMQDIApi}/Download?Arquivo={nomeArquivo}";

                    var templeteNotificacao = CriarNotificacao
                        ($"Exportação Consulta Indicadores - {filtro.MesAno.Replace("-", "/")}",
                        ApplicationConstants.TextoNotificacaoExportacaoIndicadores.Replace("{link}", linkDownload),
                        _userService.SidUsuario(),
                        "ExportarConsultaIndicadores");

                    _notificacaoService.Send(templeteNotificacao);
                }
                else
                {
                    throw new Exception("Ocorreu um erro durante o envio do arquivo para o SharePoint. Por favor, contate o administrador do sistema para assistência.");
                }

                if (byteArray != null && byteArray.Length > 0)
                {
                    return new FileContentResult(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = nomeArquivo
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public Notificacao CriarNotificacao(string titulo, string mensagem, string sidUsuario, string topico)
        {
            return new Models.Model.Notificacao
            {
                Titulo = titulo,
                Corpo = new CorpoNotificacao
                {
                    Mensagem = mensagem
                },
                CorpoEmail = new CorpoEmail
                {
                    MensagemEmail = mensagem
                },
                Audiencia = new Audiencia
                {
                    Topicos = new List<Topico>
                    {
                         new Topico
                            {
                             Nome = $"Sistema/PortalMQDI/{topico}"
                            }
                    }
                },
                CriacaoAutomatica = true,
                Claims = new List<Claims>
                {
                    new Claims
                        {
                            Type = "SID",
                            Text = sidUsuario
                        }
                }
            };
        }

        public List<string> ExportaRelatorioAnalitico(ExportarConsultaIndicadorViewModel filtro)
        {
            try
            {
                var urls = new List<string>();
                string data = filtro.MesAno.Replace("-", "/");
                var agenteEntryList = _contestacaoRepository.BuscarAgenteAsync(filtro.MesAno, CancellationToken.None).GetAwaiter().GetResult();
                var agenteProcessar = agenteEntryList.Where(c => filtro.Agentes.Count() > 0 ? filtro.Agentes.Contains(c.AgeMrid.Trim()) : true);

                var entryAgentes = _agenteRepository
                    .BuscarViewPorFiltro(filtro.MesAno, agenteProcessar.Select(c => c.Mrid).ToList(), filtro.Indicador, filtro.Violacao, true);

                var entryResultadoIndicador = _resultadoIndicadorRepository
                   .BuscarViewPorFiltro(filtro.MesAno, agenteProcessar.Select(c => c.Mrid).ToList(), filtro.Indicador, filtro.Violacao, true);

                var entryInstalacao = _instalacaoRepository
                   .ExportarInstalacao(filtro.Agentes, filtro.MesAno, true);


                var agruparInstalacao = entryInstalacao
                    .GroupBy(r => new { r.NomeCurtoInstalacao, r.TipoIndicador, r.CosId, r.AgeMrid });



                foreach (var agenteItem in agenteProcessar)
                {
                    var agenteList = entryAgentes.Where(c => c.Mrid == agenteItem.Mrid);
                    var resultadoIndicadorList = entryResultadoIndicador.Where(c => c.Mrid != agenteItem.Mrid);
                    var instalacaoList = agruparInstalacao.FirstOrDefault(c => c.Key.AgeMrid == agenteItem.Mrid);

                    var templeteBody = new TempleteUtil(TempleteEnum.RelatorioAnalitico.GetDescription());
                    var templeteItem = templeteBody.ReadHtmlFile("templete.html")
                        .Replace("{data}", filtro.MesAno.Replace("-", "/"));

                    foreach (var documentoAgente in agenteList)
                    {
                        templeteItem += templeteBody.ReadHtmlFile("itemAgenteTemplete.html")
                            .Replace("{nomeAgente}", documentoAgente.NomeLongo)
                            .Replace("{indicador}", documentoAgente.CodIndicador)
                            .Replace("{valorAnual}", documentoAgente.ValAnual.RoundToTwoDecimalPlaces())
                            .Replace("{valorMensal}", documentoAgente.ValMensal.RoundToTwoDecimalPlaces())
                            .Replace("{constestacaoAnalista}", documentoAgente.DscAnalistaConstestacao)
                            .Replace("{constestacaoOns}", documentoAgente.DscOnsConstestacao ?? "Não há comentários")
                            .Replace("{fragAnual}", documentoAgente.FlgViolacaoAnual ? "style=color:red;" : "")
                            .Replace("{fragMensal}", documentoAgente.FlgViolacaoMensal ? "style=color:red;" : "");
                    }

                    foreach (var documentoSSCL in resultadoIndicadorList)
                    {
                        var cosIdWithoutPrefix = documentoSSCL.CosId.Replace("COSR-", "");
                        Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);

                        templeteItem += templeteBody.ReadHtmlFile("itemSSCLTemplete.html")
                           .Replace("{nomeAgente}", documentoSSCL.UtrCd)
                           .Replace("{indicador}", documentoSSCL.CodIndicador)
                           .Replace("{valorAnual}", documentoSSCL.ValAnual.RoundToTwoDecimalPlaces())
                           .Replace("{valorMensal}", documentoSSCL.ValMensal.RoundToTwoDecimalPlaces())
                           .Replace("{constestacaoAnalista}", documentoSSCL.DscAnalistaConstestacao)
                           .Replace("{constestacaoOns}", documentoSSCL.DscOnsConstestacao ?? "Não há comentários")
                           .Replace("{fragAnual}", documentoSSCL.FlgViolacaoAnual ? "style=color:red;" : "")
                           .Replace("{fragMensal}", documentoSSCL.FlgViolacaoMensal ? "style=color:red;" : "")
                           .Replace("{centro}", cosIdEnum.GetDescription());
                    }
                    if (instalacaoList != null && instalacaoList.Count() > 0)
                    {
                        var instalacaoDocumento = instalacaoList.FirstOrDefault();

                        var cosIdWithoutPrefix = instalacaoDocumento.CosId.Replace("COSR-", "");
                        Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);

                        templeteItem += templeteBody.ReadHtmlFile("itemInstalacaoTemplete.html")
                           .Replace("{nomeAgente}", instalacaoDocumento.NomeCurtoInstalacao)
                           .Replace("{indicador}", instalacaoDocumento.TipoIndicador)
                           .Replace("{valorAnual}", instalacaoDocumento.ValorInstalacaoAnual.Value.RoundToTwoDecimalPlaces())
                           .Replace("{valorMensal}", instalacaoDocumento.ValorInstalacaoMensal.Value.RoundToTwoDecimalPlaces())
                           .Replace("{constestacaoAnalista}", instalacaoDocumento.InstalacaoAnalista)
                           .Replace("{constestacaoOns}", instalacaoDocumento.InstalacaoONS ?? "Não há comentários")
                           .Replace("{centro}", cosIdEnum.GetDescription());


                        foreach (var grandeza in instalacaoList)
                        {
                            var cosIdWithoutPrefix2 = grandeza.CosId.Replace("COSR-", "");
                            Enum.TryParse(cosIdWithoutPrefix2, out CentroOperacaoEnum cosIdEnum2);

                            templeteItem += templeteBody.ReadHtmlFile("itemRecursoTemplete.html")
                               .Replace("{identificador}", grandeza.Grandeza)
                               .Replace("{indicador}", grandeza.TipoIndicador)
                               .Replace("{valorAnual}", grandeza.ValorRecursoAnual.Value.RoundToTwoDecimalPlaces())
                               .Replace("{valorMensal}", grandeza.ValorRecursoMensal.Value.RoundToTwoDecimalPlaces())
                               .Replace("{constestacaoAnalista}", grandeza.RecursoAnalista)
                               .Replace("{instalacao}", grandeza.NomeCurtoInstalacao)
                               .Replace("{descricao}", grandeza.DescricaoGrandeza)
                               .Replace("{ligacaoSSC}", grandeza.Lscinf)
                               .Replace("{endProtocolo}", grandeza.NomEnderecoFisico)
                               .Replace("{constestacaoOns}", grandeza.RecursoONS ?? "Não há comentários")
                               .Replace("{centro}", cosIdEnum2.GetDescription());
                        }
                    }

                    templeteItem += templeteBody.ReadHtmlFile("styleTemplete.html");
                    var nomeArquivo = $"{filtro.MesAno}.{agenteItem.NomeCurto.Replace(" ", "-")}-{DateTime.Now.ToString("ddMMyyyyHHmm")}.pdf";
                    var arquivo = DocumentGeneratorUtil.GeneratePdfFromHtml(templeteItem, "Relatório Analítico de Contestações dos Agentes");


                    var uploadFile = _awsService.UploadFileAsync("Arquivos_Temporarios", arquivo, nomeArquivo, CancellationToken.None).GetAwaiter().GetResult();

                    if (!String.IsNullOrEmpty(uploadFile))
                    {
                        string linkDownload = $"{_settings.Value.PortalMQDIApi}/Download?Arquivo={nomeArquivo}";
                        urls.Add(linkDownload);
                    }
                }

                if (urls.Count > 0)
                {
                    var templeteNotificacao = CriarNotificacao
                ($"Relatório Analítico de Contestações dos Agentes - {filtro.MesAno.Replace("-", "/")}",
                ApplicationConstants.TextoNotificacaoExportacaoIndicadores.Replace("{link}", string.Join(", ", urls)),
                _userService.SidUsuario(),
                "ExportarConsultaIndicadores");
                    _notificacaoService.Send(templeteNotificacao);
                }
                return urls;
            }
            catch
            {
                return null;
            }
        }

        public FileContentResult ExportaMedidaNaoSupervisionada(ExportarConsultaIndicadorViewModel filtro)
        {
            var agentesEntry = _agenteRepository
                .GetAsync(c => filtro.Agentes.Contains(c.AgeMrid) && c.AnoMesReferencia == filtro.MesAno.ConvertToAnomeReferencia(), CancellationToken.None)
                .GetAwaiter().GetResult();

            var instalacaoEntry = _grandezaRepository
                .InstalacaoConsultarMedida(filtro.Agentes, filtro.MesAno.ConvertToAnomeReferencia());

            using (var workbook = new XLWorkbook())
            {
                foreach (var agente in agentesEntry.OrderBy(c => c.IdOns))
                {
                    var dados = new List<string[]>();


                    var agenteWorkshee = ExcelUtil.CreateWorksheetWithHeaders(workbook, agente.IdOns, new List<string>
                    {
                         "Identificador ONS", "Nome Instalação", "Código Instalação ", "Centro", "Descrição", "Rede"
                    });


                    var agroupInstalacao = instalacaoEntry
                        .Where(c => c.AgeMrid == agente.AgeMrid)
                        .GroupBy(c => c.IdInstalacao).Select(c => new
                        {
                            InstalacaoNome = c.FirstOrDefault().NomeInstalacao,
                            IdInstalacao = c.Key,
                            Recurso = c.ToList()
                        });

                    foreach (var instalacao in agroupInstalacao)
                    {
                        foreach (var recurso in instalacao.Recurso)
                        {
                            dados.Add(new string[] {
                                recurso.IdPonto,
                                instalacao.InstalacaoNome,
                                instalacao.IdInstalacao,
                                NomeCentro(recurso.CosId),
                                recurso.DescricaoGrandeza,
                                NomeRede(recurso.TipoRede)
                            });
                        }
                    }
                    ExcelUtil.FillWorksheetData(agenteWorkshee, dados);
                }

                string nomeArquivo = $"{filtro.MesAno}.{DateTime.Now.ToString("ddMMyyyyHHmm")}.xlsx";

                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                ms.Position = 0;
                byte[] byteArray = ms.ToArray();

                if (byteArray != null && byteArray.Length > 0)
                {
                    return new FileContentResult(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = nomeArquivo
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        #region Auxiliares
        private string NomeCentro(string cos)
        {
            var cosIdWithoutPrefix = cos.Replace("COSR-", "");
            Enum.TryParse(cosIdWithoutPrefix, out CentroOperacaoEnum cosIdEnum);

            return cosIdEnum.GetDescription();
        }

        private string NomeRede(string rede)
        {
            Enum.TryParse(rede, out TipoRedeEnum tipoRedeEnum);
            return tipoRedeEnum.GetDescription();
        }
        #endregion
    }
}
