using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Records
{
    public class RecordsListViewModel : ListScreen<RecordListInfo>
    {
        public RecordsListViewModel()
        {
            LoadData();
        }

        public override void LoadData()
        {
            var records = IoC.Get<IRecordsManager>().GetRecords();

            var context = new ObservableCollection<RecordListInfo>();

            foreach (var record in records)
            {
                context.Add(record);
            }

            Context = context;
        }

        public void NewRecord()
        {
            if (ShowDialog(new NewRecordViewModel()) == true)
            {
                LoadData();
            }
        }

        public override void SelectedContextItemDoubleClick(object context)
        {
            if (!(context is RecordListItemInfo model))
                throw new Exception("Błąd rzutowania typu wiersza");

            if (ShowDialog(new RecordViewModel(model.RecordDate, model.Name)) == true)
            {
                LoadData();
            }
        }
    }
}
