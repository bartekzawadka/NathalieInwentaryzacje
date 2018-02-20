using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class GenerateReportViewModel : DetailsScreen<RecordListInfo>
    {
        private readonly IRecordsManager _recordsManager = IoC.Get<IRecordsManager>();
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
                return Items != null && Items.Any(x => x.IsSelected);
            }
        }

        public GenerateReportViewModel(RecordListInfo context)
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

        }
    }
}
