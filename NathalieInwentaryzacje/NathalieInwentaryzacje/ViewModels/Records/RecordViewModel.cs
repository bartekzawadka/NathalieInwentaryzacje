using System;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class RecordViewModel : DetailsScreen<RecordEntryInfo>
    {
        private readonly IRecordsManager _recordsManager = IoC.Get<IRecordsManager>();

        private readonly DateTime _recordDate;
        private readonly string _recordEntryName;

        public RecordViewModel(DateTime recordDate, string recordEntryName)
        {
            _recordDate = recordDate;
            _recordEntryName = recordEntryName;
            LoadData();
        }

        public void LoadData()
        {
            Context = _recordsManager.GetRecordEntry(_recordDate, _recordEntryName);
        }

        public void Save()
        {
            _recordsManager.SaveRecord(_recordEntryName, Context);
            TryClose(true);
        }

    }
}
