using System;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Shared.Extensions
{
    public static class ExceptionExtensions
    {
        private static Random rand = new Random();

        private static int GerarNumeroAleatorio6Digitos()
        {
            return rand.Next(100000, 1000000);
        }


        public static int LogErrorWithNumber(this Exception ex, log4net.ILog log)
        {
            if (ex is TaskCanceledException)
            {
                return 0;
            }

            int numeroErro = GerarNumeroAleatorio6Digitos();
            log.Error($"Erro PortalMQDI: {numeroErro} - Mensagem: {ex.Message}");
            if (ex.InnerException != null)
            {
                log.Error($"InnerException: {ex.InnerException.Message}");
            }
            log.Error($"StackTrace: {ex.StackTrace}");

            return numeroErro;
        }
    }
}
