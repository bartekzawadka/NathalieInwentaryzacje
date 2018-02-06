using System;
using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Records
{
    [Serializable]
    public class RecordEntryColumn
    {
        [XmlAttribute("Name")]
        public string ColumnName { get; set; }

        [XmlAttribute("Value")]
        public string ColumnValue { get; set; }

        [XmlAttribute("ReadOnly")]
        public bool IsReadOnly { get; set; }
    }
}
