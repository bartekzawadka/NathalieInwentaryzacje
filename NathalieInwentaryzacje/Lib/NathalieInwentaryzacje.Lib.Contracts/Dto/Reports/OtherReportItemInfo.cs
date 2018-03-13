using NathalieInwentaryzacje.Common.Utils;
using NathalieInwentaryzacje.Lib.Contracts.Enums;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports
{
    public class OtherReportItemInfo
    {
        public OtherReportType ReportType { get; }

        public string Title => Enumerations.GetEnumDescription(ReportType);

        public bool IsSelected { get; set; }

        public OtherReportItemInfo(OtherReportType reportType)
        {
            ReportType = reportType;
        }
    }
}
