using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Appendix
{
    [Serializable]
    public class AppendixDataSet
    {
        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlArray("Entries")]
        [XmlArrayItem("Entry", typeof(AppendixDataSetEntry))]
        public AppendixDataSetEntry[] Entries { get; set; }
    }
}
