using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Templates
{
    [Serializable]
    public class TemplateColumn
    {
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
