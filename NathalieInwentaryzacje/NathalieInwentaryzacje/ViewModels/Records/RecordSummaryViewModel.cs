using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class RecordSummaryViewModel : DetailsScreen<RecordSummaryInfo>
    {
        private readonly ISummaryManager _summaryManager = IoC.Get<ISummaryManager>();

        private readonly DateTime _recordDate;
        private ObservableCollection<RecordTotalRowInfo> _customSummaryEntries = new ObservableCollection<RecordTotalRowInfo>();

        public ObservableCollection<RecordTotalRowInfo> CustomSummaryEntries
        {
            get => _customSummaryEntries;
            set
            {
                if (Equals(value, _customSummaryEntries)) return;
                _customSummaryEntries = value;
                NotifyOfPropertyChange();
            }
        }

        public RecordSummaryViewModel(DateTime recordDate)
        {
            if (recordDate == new DateTime())
                throw new ArgumentNullException(nameof(recordDate), @"Data inwentaryzacji nie została wskazana");

            _recordDate = recordDate;
            DisplayName = "Edycja pozycji raportu zbiorczego (" + recordDate.ToRecordDateString() + ")";
            GetData();
        }

        public void GetData()
        {
            var context = _summaryManager.GetSummary(_recordDate);
            var readOnlyItems = context.TotalsDataset?.Rows?.Where(x => x.IsReadOnly).ToList();
            Context = new RecordSummaryInfo
            {
                RecordDate = _recordDate,
                TotalsDataset = new RecordsTotalsInfo
                {
                    Rows = readOnlyItems
                }
            };

            if (context.TotalsDataset?.Rows != null)
                CustomSummaryEntries =
                    new ObservableCollection<RecordTotalRowInfo>(context.TotalsDataset.Rows.Where(x => !x.IsReadOnly));
        }

        public void Save()
        {
            foreach (var customSummaryEntry in CustomSummaryEntries)
            {
                customSummaryEntry.IsReadOnly = false;
            }

            var combinedList = Context.TotalsDataset?.Rows?.ToList();
            if (combinedList != null)
            {
                combinedList.AddRange(CustomSummaryEntries);
                Context.TotalsDataset.Rows = combinedList;
            }

            _summaryManager.SaveSummary(Context);
            TryClose();
        }
    }
}
