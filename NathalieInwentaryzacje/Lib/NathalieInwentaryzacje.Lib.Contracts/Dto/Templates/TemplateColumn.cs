using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Templates
{
    [Serializable]
    public class TemplateColumn
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
