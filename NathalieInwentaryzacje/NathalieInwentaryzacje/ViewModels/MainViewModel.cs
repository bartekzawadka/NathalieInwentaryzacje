using System.Threading.Tasks;
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

                if (_mainContent is ILoadable loadable)
                {
                    loadable.LoadData();
                }
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
            SyncStatusForeground = Brushes.Gray;
        }

        public void ShowRecords()
        {
            MainContent = new RecordsListViewModel{ParentScreen = this};
        }

        public void ShowTemplates()
        {
            MainContent = new TemplatesListViewModel {ParentScreen = this};
        }

        public void ShowSettings()
        {
            MainContent = new SettingsViewModel { ParentScreen = this };
        }

        public async void ShowSynchronize()
        {
            var controller = await ShowProgress("Synchronizacja", "Trwa synchronizacja inwentaryzacji i szablonów. Proszę czekać...");
            controller.SetIndeterminate();
            var result = await Synchronize();

            await controller.CloseAsync();
            if (result == Lib.Contracts.Enums.SyncStatus.UpToDate)
            {
                await ShowMessage("Zsynchronizowano", "Dane aplikacji zostały pomyślnie zsynchronizowane z repozytorium plików");
            }
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        protected override async void OnActivate()
        {
            await Synchronize();
        }

        protected override void OnViewLoaded(object view)
        {
            ShowRecords();
        }

        private async Task<SyncStatus> Synchronize()
        {
            var sm = IoC.Get<ISyncManager>();
            await sm.Synchronize();
            return UpdateStatus();
        }

        public SyncStatus UpdateStatus()
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
                    SyncStatusForeground = Brushes.Gray;
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
