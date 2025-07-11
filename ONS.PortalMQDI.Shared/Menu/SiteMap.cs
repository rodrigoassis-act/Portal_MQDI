using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ONS.PortalMQDI.Shared.Menu
{
    [Serializable]
    [XmlRoot("SiteMap")]
    public class SiteMap
    {
        [XmlAttribute]
        public string Title;
        [XmlAttribute]
        public string Description;
        [XmlAttribute]
        public string Url;
        [XmlAttribute]
        public string CodAplication;
        [XmlAttribute]
        public int NodeOrder;
        [XmlAttribute]
        public bool Published;
        [XmlAttribute]
        public bool Enabled;
        [XmlAttribute]
        public string UrlHelp;
        [XmlAttribute]
        public string PadraoBrowser;
        [XmlAttribute]
        public bool FlgPublico;
        [XmlAttribute]
        public string ClasseCSS;

        [XmlAttribute]
        public virtual string UrlIcone { get; set; }
        [XmlAttribute]
        public virtual string TipoRequisicao { get; set; }
        [XmlAttribute]
        public virtual string TipoSitemap { get; set; }


        [XmlArrayItem("SiteMap", typeof(SiteMap))]
        public List<SiteMap> Childs;
        [XmlArrayItem("Definicao", typeof(DefinicaoPermissaoSerializable))]
        public List<DefinicaoPermissaoSerializable> Definicoes;
    }
}
