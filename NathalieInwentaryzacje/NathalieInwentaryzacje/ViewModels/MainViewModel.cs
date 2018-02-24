using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using NathalieInwentaryzacje.Common.Utils;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
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
        private Brush _syncStatusBackground;
        private Brush _syncStatusForeground;
        private string _syncStatus;

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

        public Brush SyncStatusBackground
        {
            get => _syncStatusBackground;
            set
            {
                if (Equals(value, _syncStatusBackground)) return;
                _syncStatusBackground = value;
                NotifyOfPropertyChange();
            }
        }

        public Brush SyncStatusForeground
        {
            get => _syncStatusForeground;
            set
            {
                if (Equals(value, _syncStatusForeground)) return;
                _syncStatusForeground = value;
                NotifyOfPropertyChange();
            }
        }

        public string SyncStatus
        {
            get => _syncStatus;
            set
            {
                if (value == _syncStatus) return;
                _syncStatus = value;
                NotifyOfPropertyChange();
            }
        }

        public MainViewModel()
        {
            SyncStatus = Enumerations.GetEnumDescription(Lib.Contracts.Enums.SyncStatus.Unknown);
            SyncStatusBackground = Brushes.Transparent;
            SyncStatusForeground = Brushes.DarkGray;
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

        public void ShowSynchronize()
        {
            if (Synchronize() == Lib.Contracts.Enums.SyncStatus.UpToDate)
            {
                ShowMessage("Zsynchronizowano", "Dane aplikacji zostały pomyślnie zsynchronizowane z repozytorium plików");
            }
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        protected override void OnActivate()
        {
            Synchronize();
        }

        protected override void OnDeactivate(bool close)
        {
            Synchronize();
        }

        private SyncStatus Synchronize()
        {
            var sm = IoC.Get<ISyncManager>();
            sm.Synchronize();
            return UpdateStatus();
        }

        private SyncStatus UpdateStatus()
        {
            var sm = IoC.Get<ISyncManager>();
            var status = sm.GetStatus();

            SyncStatus = Enumerations.GetEnumDescription(status);

            switch (status)
            {
                case Lib.Contracts.Enums.SyncStatus.Modified:
                    SyncStatusForeground = Brushes.Black;
                    SyncStatusBackground = Brushes.DarkOrange;
                    break;
                case Lib.Contracts.Enums.SyncStatus.NotConnected:
                    SyncStatusForeground = Brushes.White;
                    SyncStatusBackground = Brushes.DarkRed;
                    break;
                case Lib.Contracts.Enums.SyncStatus.Unknown:
                    SyncStatusForeground = Brushes.DarkGray;
                    SyncStatusBackground = Brushes.Transparent;
                    break;
                case Lib.Contracts.Enums.SyncStatus.UpToDate:
                    SyncStatusBackground = Brushes.DarkGreen;
                    SyncStatusForeground = Brushes.White;
                    break;
            }

            return status;
        }
    }
}
