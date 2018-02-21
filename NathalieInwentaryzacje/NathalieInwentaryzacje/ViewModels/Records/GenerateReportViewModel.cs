using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

            Items = new ObservableCollection<GenerateReportEntryInfo>(Context.RecordsInfo.Select(recordListItemInfo =>
                new GenerateReportEntryInfo {RecordListInfo = recordListItemInfo}).ToList());
        }

        public void UpdateItems()
        {
            NotifyOfPropertyChange(nameof(IsAnySelected));
        }

        public void Generate()
        {
            var selectedItems = Items.Where(x => x.IsSelected).ToList();

            var reportInfos = _recordsManager.GetRecordsReportInfo(Context.RecordDate,
                selectedItems.Select(x => x.RecordListInfo.FilePath));

            foreach (var recordEntryReportInfo in reportInfos)
            {
                var item = selectedItems.First(x =>
                    string.Equals(x.RecordListInfo.DisplayName, recordEntryReportInfo.RecordDisplayName));

                using (var ms = new MemoryStream(_reportManager.BuildReport(recordEntryReportInfo)))
                {

                }
            }
        }
    }
}
