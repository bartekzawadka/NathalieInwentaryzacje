using System.Windows;
using Caliburn.Micro;
using NathalieInwentaryzacje.Lib.Bll.Managers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.Main;
using NathalieInwentaryzacje.Main.Interfaces;
using NathalieInwentaryzacje.ViewModels.Common;
using NathalieInwentaryzacje.ViewModels.Records;
using NathalieInwentaryzacje.ViewModels.Settings;
using NathalieInwentaryzacje.ViewModels.Templates;

namespace NathalieInwentaryzacje.ViewModels
{
    public class MainViewModel : ListScreen<RecordListInfo>, IMain
    {
        private ScreenBase _mainContent;

        public ScreenBase MainContent
        {
            get => _mainContent;
            set
            {
                if (Equals(value, _mainContent)) return;
                _mainContent = value;
                NotifyOfPropertyChange();
            }
        }

        public MainViewModel()
        {
            ShowRecords();
        }

        public void ShowRecords()
        {
            MainContent = new RecordsListViewModel();
        }

        public void ShowTemplates()
        {
            MainContent = new TemplatesListViewModel();
        }

        public void ShowSettings()
        {
            MainContent = new SettingsViewModel { ParentScreen = this };
        }

        public void Synchronize()
        {
            var sm = SyncManagerFactory.Get(SettingsManager.GetSettings());
            sm.Synchronize();
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
