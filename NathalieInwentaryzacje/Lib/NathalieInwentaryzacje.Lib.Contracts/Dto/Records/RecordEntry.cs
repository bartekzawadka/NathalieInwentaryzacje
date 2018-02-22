using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Records
{
    [Serializable]
    public class RecordEntry
    {
        [XmlAttribute("TemplateId")]
        public string TemplateId { get; set; }

        [XmlAttribute("FilePath")]
        public string FilePath { get; set; }

        [XmlAttribute("RecordDisplayName")]
        public string DisplayName { get; set; }


        [XmlAttribute("IsFilledIn")]
        public bool IsFilledIn { get; set; }
    }
}
