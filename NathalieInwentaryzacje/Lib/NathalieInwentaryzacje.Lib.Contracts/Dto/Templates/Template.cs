using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Templates
{
    [XmlRootAttribute("Template", Namespace = "", IsNullable = false)]
    public class Template
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Nazwa szablonu nie może być pusta")]
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Enabled")]
        public bool IsEnabled { get; set; } = true;

        [XmlAttribute("SumUpColumnName")]
        public string SumUpColumnName { get; set; }

        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(TemplateColumn))]
        public TemplateColumn[] Columns { get; set; }
    }
}
