using System;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordInfo
    {
        public int Year { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate{ get; set; }
        public string Name { get; set; }
    }
}
