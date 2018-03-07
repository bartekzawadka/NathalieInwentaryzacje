using System.Windows;
using System.Windows.Controls;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Additional
{
    public class PinViewModel : ScreenBase
    {
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(IsOkEnabled));
            }
        }

        public bool IsOkEnabled => !string.IsNullOrEmpty(Password);

        public bool IsPinValid { get; private set; }

        public void OnPasswordChanged(RoutedEventArgs args)
        {
            Password = ((PasswordBox)args.OriginalSource).Password;
        }

        public void Cancel()
        {
            TryClose();
        }

        public void Ok()
        {
            IsPinValid = string.Equals(Properties.Settings.Default.RecordDeletePin, Password);
            TryClose(true);
        }
    }
}
