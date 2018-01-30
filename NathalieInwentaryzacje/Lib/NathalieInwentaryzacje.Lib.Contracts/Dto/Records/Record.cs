using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Records
{
    [XmlRoot("Record", Namespace = "", IsNullable = false)]
    public class Record
    {
        [XmlAttribute("RecordDate")]
        public DateTime RecordDate { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray("Entries")]
        [XmlArrayItem("Entry", typeof(RecordEntry))]
        public RecordEntry[] Entries { get; set; }
    }
}
