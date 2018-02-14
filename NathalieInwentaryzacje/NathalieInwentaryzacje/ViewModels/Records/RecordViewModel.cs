using System;
using System.IO;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
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

        public void PrintReport()
        {
            var buff = IoC.Get<IReportManager>().BuildReport(new RecordEntryReportInfo(Context.RecordDateText,
                Context.RecordDisplayName, Context.RecordEntryTable), 2);

            if (buff == null) return;

            using (var fs = File.Open(@"D:\itext.pdf", FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(buff, 0, buff.Length);
            }
        }

    }
}
