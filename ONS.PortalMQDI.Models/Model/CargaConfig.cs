namespace ONS.PortalMQDI.Models.Model
{
    public static class CargaConfig
    {
        public static string Mensagem { get; set; }
        public static string Status { get; set; }
        public static int TotalAgente { get; set; }
        public static int AgenteProcessado { get; set; }
        public static double? Poncentagem { get; set; }

        public static void Iniciar()
        {
            AgenteProcessado = 0;
            Mensagem = string.Empty;
            Status = string.Empty;
            TotalAgente = 0;
        }
    }
}
