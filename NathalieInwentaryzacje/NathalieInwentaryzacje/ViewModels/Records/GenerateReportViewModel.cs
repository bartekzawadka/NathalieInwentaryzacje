using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class GenerateReportViewModel : DetailsScreen<GenerateReportsInfo>
    {
        private readonly IRecordsManager _recordsManager = IoC.Get<IRecordsManager>();
        private readonly IReportManager _reportManager = IoC.Get<IReportManager>();
        private ObservableCollection<GenerateReportEntryInfo> _items;

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

        public bool IsAnySelected
        {
            get
            {
                return Items != null && Items.Any(x => x.IsSelected) && Context.IsValid;
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

            _reportManager.GenerateReports(selectedItems, Context.ReportsSaveDir);
            Process.Start(Context.ReportsSaveDir);

            TryClose();
        }
    }
}
