using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Summary
{
    [XmlRoot("Summary", Namespace = "", IsNullable = false)]
    public class Summary
    {
        [XmlAttribute("Sum")]
        public decimal Sum { get; set; }

        [XmlArray("Entries")]
        [XmlArrayItem("Entry", typeof(SummaryEntry))]
        public SummaryEntry[] Entries { get; set; }
    }
}
