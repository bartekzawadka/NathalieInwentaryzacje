namespace NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary
{
    public class RecordTotalRowInfo
    {
        public string Name { get; set; }

        public string AdditionalInfo { get; set; }

        public decimal Value { get; set; }

        public bool IsReadOnly { get; set; } = true;
    }
}
