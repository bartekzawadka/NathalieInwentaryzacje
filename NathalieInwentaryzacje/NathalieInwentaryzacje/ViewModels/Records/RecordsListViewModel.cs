using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class RecordsListViewModel : ListScreen<RecordListInfo>
    { 
        public override async void LoadData()
        {
            var ctrl = await ShowProgress("Ładowanie", "Trwa ładowanie inwentaryzacji. Proszę czekać...", true);
            ctrl.SetIndeterminate();

            IEnumerable<RecordListInfo> records = null;

            try
            {
                records = await IoC.Get<IRecordsManager>().GetRecords();
            }
            finally
            {
                var context = new ObservableCollection<RecordListInfo>();
                if (records != null)
                {

                    foreach (var record in records)
                    {
                        context.Add(record);
                    }

                    Context = context;
                }

                await ctrl.CloseAsync();
            }

            var mainScreen = ParentScreen as MainViewModel;
            mainScreen?.UpdateStatus();
        }

        public void NewRecord()
        {
            if (ShowDialog(new RecordViewModel()) == true)
            {
                LoadData();
            }
        }

        public void GenerateReport(RecordListInfo context)
        {
            ShowDialog(new GenerateReportViewModel(new GenerateReportsInfo
            {
                RecordDate = context.RecordDate,
                RecordsInfo = context.RecordsInfo
            }));
        }

        public void AddRecordEntries(RecordListInfo context)
        {
            ShowDialog(new RecordViewModel(context));
            LoadData();
        }

        public override void SelectedContextItemDoubleClick(object context)
        {
            if (!(context is RecordListItemInfo model))
                throw new Exception("Błąd rzutowania typu wiersza");

            IoC.Get<IRecordsManager>().OpenRecordFileEdit(model.RecordDate, model.FilePath);
            LoadData();
        }
    }
}
