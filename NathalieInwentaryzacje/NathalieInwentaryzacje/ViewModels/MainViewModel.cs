using System;
using System.Windows;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.Main.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels
{
    public class MainViewModel : ListScreen, IMain
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public MainViewModel()
        {
            LoadData();
        }

        public void ShowName()
        {
            Name = "Hello! :)";
        }

        protected override void LoadData()
        {
            var records = IoC.Get<IRecordsManager>().GetRecords();
            Console.WriteLine(records);
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
