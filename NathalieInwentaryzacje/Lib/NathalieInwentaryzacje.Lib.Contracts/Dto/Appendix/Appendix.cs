using System.Xml.Serialization;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Appendix
{
    [XmlRoot("Appendix", Namespace = "", IsNullable = false)]
    public class Appendix
    {
        [XmlAttribute("Number")]
        public int AppendixNumber { get; set; }

        [XmlArray("DataSets")]
        [XmlArrayItem("DataSet", typeof(AppendixDataSet))]
        public AppendixDataSet[] DataSets { get; set; }
    }
}
