using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ONS.PortalMQDI.Shared.Menu
{
    public static class MenuHelper
    {

        public static string GetSiteMap(List<string> operacao)
        {
            var siteMap = ParseSiteMapXML(operacao);
            var siteMapSerializado = string.Empty;

            siteMapSerializado = JsonConvert.SerializeObject(siteMap.OrderBy(c => c.Title));

            return siteMapSerializado;
        }

        public static List<SiteMap> ParseSiteMapXML(List<string> operacao)
        {
            SiteMap MenuCompleto;
            List<SiteMap> siteMaps = new List<SiteMap>();

            string FullPathFileName = Path.Combine(Directory.GetCurrentDirectory(), "Sitemap.xml");


            using (StreamReader sr = new StreamReader(FullPathFileName, Encoding.UTF8))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(SiteMap));
                MenuCompleto = (SiteMap)serializador.Deserialize(sr);
            }

            siteMaps = MenuCompleto.Childs.Where(c => c.Definicoes.Any(x => operacao.Contains(x.Nome))).ToList();


            return siteMaps;
        }

        private static bool PermissionamentoMenu(SiteMap sitemap, List<string> permissaoLista, params string[] chaves)
        {
            bool retorno = sitemap.Definicoes.Any(d => permissaoLista.Any(p => p.Equals(d.Nome, StringComparison.InvariantCultureIgnoreCase))) || (sitemap.Childs != null && sitemap.Childs.Any(c => PermissionamentoMenu(c, permissaoLista, chaves)));
            if (sitemap.Childs != null && sitemap.Childs.Count > 0)
            {
                for (int i = 0; i < sitemap.Childs.Count; i++)
                {
                    var exibir = PermissionamentoMenu(sitemap.Childs[i], permissaoLista, chaves);
                    if (!exibir)
                    {
                        sitemap.Childs.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (retorno && !string.IsNullOrWhiteSpace(sitemap.Url) && sitemap.Url.Contains("{"))
            {
                foreach (var chave in chaves)
                {
                    sitemap.Url = sitemap.Url.Replace("{" + chave + "}", "xx");
                }
            }

            return retorno;
        }
    }
}
