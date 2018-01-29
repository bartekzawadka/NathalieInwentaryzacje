using System.Windows;
using Autofac;
using Caliburn.Micro.Autofac;
using NathalieInwentaryzacje.Lib.Bll;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels;

namespace NathalieInwentaryzacje.Main
{
    public class AppBootstrapper : AutofacBootstrapper<MainViewModel>
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterType<RecordsManager>().As<IRecordsManager>();
        }
    }
}
