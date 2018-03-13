using System.ComponentModel;

namespace NathalieInwentaryzacje.Lib.Contracts.Enums
{
    public enum OtherReportType
    {
        [Description("Załącznik")]
        Appendix = 1,

        [Description("Raport zbiorczy")]
        Summary = 2
    }
}
