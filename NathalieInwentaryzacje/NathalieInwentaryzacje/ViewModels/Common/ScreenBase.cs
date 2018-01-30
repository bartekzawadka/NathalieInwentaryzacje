using Caliburn.Micro;
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
    }
}
