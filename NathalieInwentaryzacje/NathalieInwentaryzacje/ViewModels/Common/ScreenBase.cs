using System.Threading.Tasks;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using NathalieInwentaryzacje.Main;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public abstract class ScreenBase : Conductor<object>
    {
        protected static CustomWindowsManager WindowManager => IoC.Get<IWindowManager>() as CustomWindowsManager;

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
            WindowManager.RemoveWindow(this);
        }

        public Task<MessageDialogResult> ShowMessage(string title, string message)
        {
            var view = WindowManager.GetWindowForModel(this);
            return view.ShowMessageAsync(title, message);
        }

        public bool? ShowDialog(ScreenBase viewModel)
        {
            return WindowManager.ShowDialog(viewModel);
        }

        public virtual void SelectedContextItemDoubleClick(object context)
        {

        }
    }
}
