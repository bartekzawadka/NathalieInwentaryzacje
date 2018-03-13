using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class GenerateReportViewModel : DetailsScreen<GenerateReportsInfo>
    {
        private readonly IAppendixManager _appendixManager = IoC.Get<IAppendixManager>();
        private readonly ISummaryManager _summaryManager = IoC.Get<ISummaryManager>();
        private readonly IReportManager _reportManager = IoC.Get<IReportManager>();


        private ObservableCollection<GenerateReportEntryInfo> _items;
        private ObservableCollection<OtherReportItemInfo> _otherReports;

        public ObservableCollection<GenerateReportEntryInfo> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(IsAnySelected));
            }
        }

        public ObservableCollection<OtherReportItemInfo> OtherReports
        {
            get => _otherReports;
            set
            {
                if (Equals(value, _otherReports)) return;
                _otherReports = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsAnySelected
        {
            get
            {
                return (Items != null && Items.Any(x => x.IsSelected) ||
                        (OtherReports != null && OtherReports.Any(x => x.IsSelected))) && Context.IsValid;
            }
        }

        public GenerateReportViewModel(GenerateReportsInfo context)
        {
            Context = context;

            var list = new ObservableCollection<GenerateReportEntryInfo>(Context.RecordsInfo.Where(x => x.IsFilledIn)
                .Select(recordListItemInfo =>
                    new GenerateReportEntryInfo { RecordListInfo = recordListItemInfo }).ToList());
            foreach (var generateReportEntryInfo in list)
            {
                generateReportEntryInfo.IsSelected = true;
            }

            Items = list;

            var otherList = new ObservableCollection<OtherReportItemInfo>(Context.OtherReportsInfo);
            OtherReports = otherList;
        }

        public void UpdateItems()
        {
            NotifyOfPropertyChange(nameof(IsAnySelected));
        }

        public void ChooseReportsDir()
        {
            var dirDialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            if (dirDialog.ShowDialog() == DialogResult.OK)
            {
                Context.ReportsSaveDir = dirDialog.SelectedPath;
                NotifyOfPropertyChange(nameof(Context));
                NotifyOfPropertyChange(nameof(IsAnySelected));
            }
        }

        public void Generate()
        {
            var selectedItems = Items.Where(x => x.IsSelected).ToList();

            if (selectedItems.Any())
                _reportManager.GenerateReports(Context.RecordDate, selectedItems, Context.ReportsSaveDir);

            var selectedOthers = OtherReports.Where(x => x.IsSelected).ToList();

            if (selectedOthers.Any())
            {
                foreach (var otherReportItemInfo in selectedOthers)
                {
                    switch (otherReportItemInfo.ReportType)
                    {
                        case OtherReportType.Appendix:
                            _reportManager.GenerateAppendix(_appendixManager.GetAppendix(Context.RecordDate),
                                Context.ReportsSaveDir);
                            break;
                        case OtherReportType.Summary:
                            _reportManager.GenerateSummary(_summaryManager.GetSummary(Context.RecordDate),
                                Context.ReportsSaveDir);
                            break;
                    }
                }
            }

            Process.Start(Context.ReportsSaveDir);

            TryClose();
        }
    }
}
