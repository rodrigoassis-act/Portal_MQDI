using System;

namespace ONS.PortalMQDI.Models.Model
{
    public class StatusCarga
    {
        public string Mensagem { get; set; }
        public string Status { get; set; }
        public double? PoncetagemAgente { get; set; }

        public StatusCarga()
        {
            Mensagem = CargaConfig.Mensagem;
            Status = CargaConfig.Status;

            if (CargaConfig.Poncentagem.HasValue)
            {
                PoncetagemAgente = Math.Round(CargaConfig.Poncentagem.Value, 2);
            }
        }
    }
}
