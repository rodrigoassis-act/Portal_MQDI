using System;
using System.IO;

namespace ONS.PortalMQDI.Shared.Utils
{
    public class TempleteUtil
    {
        private string FolderTemplete { get; set; }

        public TempleteUtil(string folderTemplete)
        {
            FolderTemplete = folderTemplete;
        }

        public string ReadHtmlFile(string nomeArquivo)
        {
            string content = string.Empty;
            string fullPath = Path.Combine(FolderTemplete, nomeArquivo);

            try
            {
                content = File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
            }

            return content;
        }
    }
}
