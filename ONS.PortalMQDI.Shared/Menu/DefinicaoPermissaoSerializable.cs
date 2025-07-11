using System;
using System.Xml.Serialization;

namespace ONS.PortalMQDI.Shared.Menu
{
    [Serializable]
    [XmlRoot("Definicao")]
    public class DefinicaoPermissaoSerializable
    {
        [XmlAttribute]
        public string Nome;

        [XmlAttribute]
        public string Descricao;

        [XmlAttribute("Tipo")]
        public string IdTipoPermissao;
    }
}
