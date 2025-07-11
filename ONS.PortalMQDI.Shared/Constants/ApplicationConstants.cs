namespace ONS.PortalMQDI.Shared.Constants
{
    public static class ApplicationConstants
    {

        // mqdi

        public const string StatusSGRAtendidaImedito = "Atendida de Imediato";
        public const string StatusSGRAtendida = "Atendida";
        public const string StatusSGRCancelada = "Cancelada";

        public const string FlgROGerado = "FlgROGerado";
        public const string FlgViolacaoAnual = "FlgViolacaoAnual";
        public const string FiltroViolacao = "FlgViolacao";
        public const string FiltroMesEAnoIndicador = "DinReferenciaIndicador";
        public const string FiltroInstalacao = "OrigemIndicador.CodInstalacao";
        public const string FiltroCodAgente = "OrigemIndicador.CodAgente";
        public const string FiltroCodCentro = "OrigemIndicador.CodCentro";
        public const string FiltroCodigoTpIndicador = "TpIndicador.CodTpIndicador";
        public const string FiltroTodosOsConcentradores = "FiltroTodosOsConcentradores";
        public const string FiltroTodasAsInstalacoes = "FiltroTodasAsInstalacoes";
        public const string FiltroAgenteEAtivos = "FiltroAgenteEAtivos";
        public const string FiltroSomenteInstalacao = "FiltroSomenteInstalacao";
        public const string FiltroSomenteConcentrador = "FiltroSomenteConcentrador";
        public const string FiltroSomenteAgente = "FiltroSomenteAgente";
        public const string FiltroAgrupamentoPorAgente = "FiltroAgrupamentoPorAgente";
        public const string FiltroConcentradorOuAgente = "FiltroConcentradorOuAgente";
        public const string FiltroApenasConcentrador = "FiltroApenasConcentrador";
        public const string FiltroAcompanhamentoGeral = "FiltroAcompanhamentoGeral";
        public const string FiltroContestacaoComAnaliseONS = "FlgComAnalise";
        public const string FiltroTodosOsRecursos = "FiltroTodosOsRecursos";
        public const string FiltroApenasContestacoes = "FiltroApenasContestacoes";
        public const string FiltroApenasAgentesContestacoes = "FiltroApenasAgentesContestacoes";
        public const string FiltroApenasQDsContestacoes = "FiltroApenasQDsContestacoes";
        public const string FiltroApenasRecursosContestacoes = "FiltroApenasRecursosContestacoes";
        public const string FiltroAgente = "OrigemIndicador.CodAgente";
        public const string FiltroRetiraRecursos = "FiltroRetiraRecursos";
        public const string FiltroOpcaoPrazoContestacao = "FiltroOpcaoPrazoContestacao";
        public const string FiltroTpResultadoContestacao = "TpResultadoContestacao";
        public const string FiltroTpRedeDeRecurso = "IdTpRede";
        public const string FiltroDataAlertaFimDePrazo = "FiltroDataAlertaFimDePrazo";

        public const string opcaoPrazoContestacaoAntes = "antesDoPrazo";
        public const string opcaoPrazoContestacaoDepois = "depoisDoPrazo";

        public const string ParaMetroDiaUtilOuCorrido = "Dia Corrido / Dia útil";
        public const string ValorParametroParaDiaUtil = "Dia Útil";
        public const string ValorParametroParaDiaCorrido = "Dia Corrido";

        public const string CabecalhoAgente = "Agente, Indicador, Anual (%), Mensal (%), Anual (%) Violado, Mensal (%) Violado, Análise Agente, Análise ONS, Análise Agente Descrição, Análise ONS Descrição,  ";
        public const string CabecalhoConcentrador = "SSCL/CD,  Indicador, Anual (%), Mensal (%), Anual (%) Violado, Mensal (%) Violado, Análise Agente, Análise ONS, Análise Agente Descrição ,  Análise ONS Descrição";
        public const string CabecalhoInstalacao = "Instalação, Indicador, Anual (%), Mensal (%), Anual (%) Violado, Mensal (%) Violado, Análise Agente, Análise ONS, Análise Agente Descrição, Análise ONS Descrição";
        public const string CabecalhoGrandeza = "Identificador ONS, Descrição, Indicador, Anual (%), Mensal (%), Anual (%) Violado, Mensal (%) Violado, Instalação, Rede, Ligacao SSC, Endereco no Protocolo, Análise Agente, Análise ONS, Análise Agente Descrição, Análise ONS Descrição, ";

        public const string ParametroLiberacaoAgente = "Liberação dos indicadores para os Agentes";
        public const string ParametroTerminoPeriodoContestacao = "Prazo em dias para consistência dos Agentes após liberação";
        public const string ParametroTerminoPeriodoAnaliseContestacao = "Prazo em dias para consolidação pelo ONS após consistência";
        public const string ParametroAvisoTerminoPeriodoAnaliseContestacao = "Dias de antecedência para iniciar o alerta de fim do prazo de consolidação";
        public const string ParametroNumeroDeMesesParaViolacaoAgentes = "Número de meses subsequentes com violações para emissão do RO para Agentes";
        public const string ParametroNumeroDeMesesParaViolacaoInstalacoes = "Número de meses subsequentes com violações para emissão do RO para Instalações";
        public const string ParametroNumeroDeMesesParaViolacaoCD = "Número de meses subsequentes com violações para emissão do RO para Concentrador de dados";


        //segunda data calendario
        public const string TextoTerminoPeriodoContestacao = "Término do Período de Contestação pelos Agentes";
        //terceira data calendario
        public const string TextoAvisoTerminoPeriodoAnaliseContestacao = "Início de Alerta de Fim de Prazo de Consolidação pelo ONS";
        //quarta data calendario
        public const string TextoTerminoPeriodoAnaliseContestacao = "Termino do Período de Consolidação do ONS";

        public const int valorDeMesesAnteriores = 12;
        public const int NumeroTotalDeTpIndicadores = 3;



        public const int ContestacaoAprovada = 1;
        public const string StrContestacaoAprovada = "Aprovado";
        public const int ContestacaoReprovada = 2;

        public const string URLAtualizacaoDeCalendario = "URLAtualizacaoDeCalendario";
        public const string URLVerificaLiberacaoIndicadores = "URLVerificaLiberacaoIndicadores";
        public const string URLNotificacao = "URLNotificacao";
        public const string HabilitarVerificacaoIndicadores = "HabilitarVerificacaoIndicadores";
        public const string HabilitarAtualizacaoDeCalendario = "HabilitarAtualizacaoDeCalendario";
        public const string HabilitarNotificacao = "HabilitarNotificacao";

        public const string NomeIndicadorQualidade = "Qualidade";
        public const string AbreviaturaIndicadorQualidade = "QUALID";
        public const string NomeIndicadorDisponibilidade = "Disponibilidade";
        public const string AbreviaturaIndicadorDisponibilidade = "DISPON";
        public const string AbreviaturaIndicadorConcentrador = "DCD";
        public const string NomeAplicacaoRelatorio = "_MQDI_";
        public const string NomeViolacaoRelatorio = "VIOL_";
        public const string SiglaRelatorioAcompanhamento = "RAcDQ";
        public const string SiglaRelatorioViolacao = "RAvDQ";

        public const string DataInicialIndicadores = "01/";
        public const string SeparadorTracoBaixo = "_";

        public const string InicioTextoNotificacao = "Atenção, existem violações acumuladas no período, no Centro de Operação ";
        public const string FimTextoNotificacao = " , referentes ao ativo: ";
        public const string TextoNotificacaoExportacaoIndicadores = "A exportação da Consulta de Indicadores está agora disponível. Você pode acessar o relatório completo através do link fornecido. Caso tenha alguma dúvida ou problema, entre em contato com o administrador. {link}";
        public const string TextoNotificacaoFimGeracaoRelatorioFinal = "Atenção, geração do Relatório {3} do mês/ano {0} solicitada em {1}, do centro de operação {2} terminou para agente(s): ";
        public const string TextoNotificacaoFimGeracaoRelatorioFinalPOP = "Atenção, geração do Relatório {3} do mês/ano {0} solicitada em {1}, do centro de operação {2} terminou para o total de {4} agente(s)";
        public const string TextoNotificacaoFimGeracaoRelatorioFinalErro = "Atenção, geração do Relatório {3} do mês/ano {0} solicitada em {1}, do centro de operação {2} falhou para agente(s): ";
        public const string TextoNotificacaoFimGeracaoRelatorioFinalErroPOP = "Atenção, geração do Relatório {3} do mês/ano {0} solicitada em {1}, do centro de operação {2} falhou  para o total de {4} agente(s)";
        public const string TextoNotificacaoFimGeracaoRelatorioAnalitico = "Atenção, geração de Relatório Analítico de Contestações solicitada em {0}, para Ano/Mês {1} e Centro {2} terminou. ";
        public const string TextoNotificacaoFimGeracaoRelatorioAnaliticoErro = "Atenção, geração de Relatório Analítico de Contestações solicitada em {0}, para Ano/Mês {1} e Centro {2} falhou. ";
        public const string TextoNotificacaoFimExportacao = "Atenção, exportação solicitada em {0} terminou para agente(s): ";
        public const string TextoNotificacaoFimExportacaoErroGerarArquivo = "Atenção, não foi possivel criar arquivo de exportação.";
        public const string NomeTemplateROSharepoint = "NomeTemplateROSharepoint";
        public const string LimparTabelasTemporarias = "limparTabelasTemporarias";
        public const string NomePastaTemplateSharepoint = "Templates";
        public const string NomePastaTemporariaSharepoint = "NomePastaTemporariaSharepoint";
        public const string UrlSicop = "sicop.visualizarIntervencao";
        public const string UrlWebSite = "urlWebSite";


        public const string DateFormatMesAno = "MM/yyyy";
        public const string SeparadorTraco = "-";
        //mqdi


        public const string RegexProtocolo = "RegexProtocolo";
        public const string RegexNumeroDocumento = "RegexNumeroDocumento";

        public const string SharePointConfig = "sharepoint.config";
        public const string InicioPortalMQDI = "InicioPortalMQDI";
        public const string AngularURL = "AngularURL";
        public const string RegexProtocoloAnteriorONS = "regexProtocoloAnteriorONS";
        public const string RegexProtocoloAnteriorRE = "regexProtocoloAnteriorRE";
        public const string RegexProtocoloAnteriorONS_RA = "regexProtocoloAnteriorONS-RA";
        public const string RegexProtocoloAnteriorRE_RA = "regexProtocoloAnteriorRE-RA";
        public const string RegexParaEncontrarExtensaoDeDocumentos = @"\.[a-zA-Z]{3,}";
        public const string WebSiteName = "WebSiteName";

        public const string CodigoNulo = "XXX";

        public const string TpIndicadorDRSC = "DRSC";
        public const string TpIndicadorQRSC = "QRSC";
        public const string TpIndicadorDCD = "DCD";

        public const string PontoConexaoLT = " LT ";
        public const string PontoConexaoKV = " kV";

        public const string PontoConexaoSeparadorEspaco = " ";
        public const string PontoConexaoSeparadorTraco = " - ";

        public const string SufixoSolicitacaoDuplicada = " Cópia";
        public const string SufixoSolicitacaoRevisão = " Revisão";
        public const string SufixoSolicitacaoRevalidacao = " Revalidação";
        public const string NomeTemporárioParaRevisão = " revisão temporária ";
        public const string TipoConexaoA1 = "Em subestação existente/futura";
        public const string TipoConexaoA2 = "Em seccionamento ou derivação (tape)";
        public const int QuantidadeDeCamposFormularioEstudos = 9;
        public const int NumeroMinimoDiasParaRevalidacao = 90;
        public const int NumeroMaximoDiasParaRevalidacao = 120;
        public const string Carta = "CARTA";
        public const string PastaTemp = "temp/";
        public const int IdInicialCampoEstudos = 934;

        public const string PrefixoMensagemLinhaTabela = " (Linha ";
        public const string SufixoMensagemLinhaTabela = ") ";
        public const string SeparadorHifen = " - ";

        public const string AbaInformacoes = "Aba Informações";
        public const int IndexArquivoOutorga = 2;
        public const int IndexDespachoOutorga = 0;




        public const string PrimeiraSecao = "FirstSection";
        public const string TamFonteSecao = "16";
        public const string TamFonteSubSecao = "14";
        public const string FolhaA4 = "A4";
        public const string FolhaA5 = "A5";
        public const string FolhaCarta = "CARTA";
        public const string OrientacaoRetrato = "RETRATO";
        public const string OrientacaoPaisagem = "PAISAGEM";
        public const string InicioSecao = "<h1>";
        public const string FimSecao = "</h1>";
        public const string InicioSubSecao = "<h2>";
        public const string FimSubSecao = "</h2>";

        public const string campoText = "text";
        public const string campoTextArea = "textarea";
        public const string campoNumber = "number";
        public const string campoSelect = "select";
        public const string campoDate = "date";
        public const string campoFile = "file";

        public const char fileSeparator = '|';

        public const int NumeroCasasDecimais = 2;
        public const string PrefixoTabela = "tb_";
        public const string SolicitacoesFormulariosString = "SolicitacoesFormularios";

        public const string EnDateFormat = "yyyy/MM/dd";
        public const string DateFormat = "dd/MM/yyyy";
        public const string AnoMesmoFormat = "yyyy/MM";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public const int TamanhoMaximoMensagem = 200;

        public const string SeparadorMensagem = ": ";
        public const string SeparadorBarra = "/";
        public const string Prazo15Dias = " 15";
        public const string Prazo30Dias = " 30";
        public const string Prazo90Dias = " 90";
        public const string faltamMenosDe = "Faltam menos de ";
        public const string DiasParaOFinalDoProcesso = " dias para o final do processo. ";

        public const int PrazoValidadeParecer = 90;
        public const int PrazoPedidoRevalidacao = 120;

        public const string TipoEscopoPortalMQDI = "APLICACAO";
        public const string TipoEscopoPortalMQDIConfig = "TipoEscopo";
        public const string TipoEscopoAgenteMQDIConfig = "TipoEscopoAgente";
        public const string IdEscopoPortalMQDIConfig = "IdEscopo";

        public const string MensagemPendenciaInformadaInicio = "Pendência informada em ";


        public const int LimiteDeCaracteresDescricaoContestacao = 2000;
        public const int LimiteDeCaracteresDescricaoComentarioResultado = 2000;



        public const string SubPastaDocumentosNoSharePoint = "/Documentos";
        public const string SubPastaAnexosNoSharePoint = "/Anexos";
        public const string SubPastaAnalisesTecnicasNoSharePoint = "/Analises Tecnicas";
        public const string SubPastaNotasAnalistasNoSharePoint = "/Notas Analista";

        public const string PastaArquivosTemporatios = "Temp";

        public const string ScheduleId = "VerificacaoDocumentosPortalMQDI";
        public const string WEB_CONFIG_VERIFICACAODOCUMENTOSEMITIDOS_SCHEDULE_CRON_TOTAL = "verificacaoDocumentos.schedule.cron.cache.total";
        public const string WEB_CONFIG_VERIFICACAODOCUMENTOSEMITIDOS_SCHEDULE_URL_WEBSERVICE = "ons.schedule.webapi.url";

        public const string TextAnalogico = "Analógico";
        public const string TextDigital = "Digital";
        public const string EqualAnalogico = "pas";

    }
}
