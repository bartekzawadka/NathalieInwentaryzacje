using System.Collections;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Reports
{
    public class ReportViewModel : ScreenBase
    {
        private RecordEntryReportInfo _data;

        public RecordEntryReportInfo Data
        {
            get { return _data; }
            set
            {
                if (Equals(value, _data)) return;
                _data = value;
                NotifyOfPropertyChange();
            }
        }

        public ReportViewModel(RecordEntryReportInfo data)
        {
            Data = data;
        }
    }
}
