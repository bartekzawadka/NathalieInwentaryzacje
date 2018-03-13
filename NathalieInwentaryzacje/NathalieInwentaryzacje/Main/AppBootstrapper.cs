using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Autofac;
using NathalieInwentaryzacje.Lib.Bll.Managers;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using NathalieInwentaryzacje.ViewModels;
using NathalieInwentaryzacje.ViewModels.Main;

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

            var paths = SettingsManager.GetDataLocationInfo();

            builder.RegisterInstance(new CustomWindowsManager()).As<IWindowManager>();
            builder.RegisterInstance(new RecordsManager(paths)).As<IRecordsManager>();
            builder.RegisterInstance(new TemplatesManager(paths)).As<ITemplatesManager>();
            builder.RegisterInstance(new ReportManager(paths)).As<IReportManager>();
            builder.RegisterInstance(new AppendixManager(paths)).As<IAppendixManager>();
            builder.RegisterInstance(new SummaryManager(paths)).As<ISummaryManager>();
            builder.RegisterInstance(new SyncManager(SettingsManager.GetSettings(), paths)).As<ISyncManager>();
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowException(e.Exception);
            e.Handled = true;
        }

        public static void ShowException(Exception ex)
        {
            Execute.OnUIThread(() =>
            {
                var e = ex;
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                {
                    e = ex.InnerException;
                }
                IoC.Get<IWindowManager>().ShowDialog(new ErrorViewModel(e));
            });
        }
    }
}
