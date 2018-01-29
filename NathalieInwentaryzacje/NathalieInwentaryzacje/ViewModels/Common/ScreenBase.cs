using Caliburn.Micro;

namespace NathalieInwentaryzacje.ViewModels.Common
{
    public class ScreenBase : PropertyChangedBase
    {
        protected static WindowManager WindowManager => IoC.Get<IWindowManager>() as WindowManager;
    }
}
