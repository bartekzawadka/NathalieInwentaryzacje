using System.Windows;
using System.Windows.Controls;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Main;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Settings
{
    public class SettingsViewModel : DetailsScreen<SettingsInfo>
    {
        public string Password
        {
            get;
            set;
        }

        public SettingsViewModel()
        {
            Load();
        }

        private void Load()
        {
            Context = SettingsManager.GetSettings();
            Password = Context.RepoPassword;
        }

        public void OnPasswordChanged(RoutedEventArgs args)
        {
            Password = ((PasswordBox)args.OriginalSource).Password;
        }

        public async void Save()
        {
            if (string.IsNullOrEmpty(Password))
            {
                await ShowMessage("Brak hasła", "Hasło do repozytorium nie zostało określone", true);
                return;
            }

            Context.RepoPassword = Password;

            SettingsManager.SaveSettings(Context);

            await ShowMessage("Zapis", "Pomyślnie zapisano ustawienia", true);
            Load();
        }
    }
}
