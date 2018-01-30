using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Templates
{
    [XmlRootAttribute("Template", Namespace = "", IsNullable = false)]
    public class TemplateInfo
    {
        private bool _isEnabled = true;

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Enabled")]
        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(TemplateColumn))]
        public TemplateColumn[] Columns { get; set; }
    }
}
