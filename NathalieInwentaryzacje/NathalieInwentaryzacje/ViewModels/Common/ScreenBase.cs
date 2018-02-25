using System.Threading.Tasks;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using NathalieInwentaryzacje.Main;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public abstract class ScreenBase : Conductor<object>
    {
        protected static CustomWindowsManager WindowManager => IoC.Get<IWindowManager>() as CustomWindowsManager;

        public ScreenBase ParentScreen { get; set; }

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
            WindowManager.RemoveWindow(this);
        }

        public Task<MessageDialogResult> ShowMessage(string title, string message, bool showOnParentScreen = false)
        {
            var screen = this;
            if (showOnParentScreen)
                screen = ParentScreen;
            var view = WindowManager.GetWindowForModel(screen);
            return view.ShowMessageAsync(title, message);
        }


        public bool? ShowDialog(ScreenBase viewModel)
        {
            return WindowManager.ShowDialog(viewModel);
        }

        public Task<ProgressDialogController> ShowProgress(string title, string message, bool showOnParentScreen = false)
        {
            var screen = this;
            if (showOnParentScreen)
                screen = ParentScreen;
            var view = WindowManager.GetWindowForModel(screen);
            return view.ShowProgressAsync(title, message);
        }

        public virtual void SelectedContextItemDoubleClick(object context)
        {

        }
    }
}
