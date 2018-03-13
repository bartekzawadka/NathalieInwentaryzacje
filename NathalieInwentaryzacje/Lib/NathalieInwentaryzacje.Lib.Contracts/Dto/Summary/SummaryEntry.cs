using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Summary
{
    [Serializable]
    public class SummaryEntry
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("AdditionalInfo")]
        public string AdditionalInfo { get; set; }

        [XmlAttribute("Value")]
        public decimal Value { get; set; }

        [XmlAttribute("IsReadOnly")]
        public bool IsReadOnly { get; set; }
    }
}
