using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Records
{
    [Serializable]
    public class RecordEntry
    {
        [XmlArray("Columns")]
        [XmlArrayItem("Column", typeof(RecordEntryColumn))]
        public RecordEntryColumn[] Columns { get; set; }
    }
}
