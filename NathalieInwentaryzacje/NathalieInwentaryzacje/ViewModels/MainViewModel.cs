using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.Main.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;
using NathalieInwentaryzacje.ViewModels.Records;

namespace NathalieInwentaryzacje.ViewModels
{
    public class MainViewModel : ListScreen<RecordInfo>, IMain
    {
        public MainViewModel()
        {
            LoadData();
        }

        public override void LoadData()
        {
            var records = IoC.Get<IRecordsManager>().GetRecords();

            var context = new ObservableCollection<RecordInfo>();

            foreach (var record in records)
            {
                context.Add(record);
            }

            Context = context;
        }

        public void NewRecord()
        {
            WindowManager.ShowDialog(new NewRecordViewModel());
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
