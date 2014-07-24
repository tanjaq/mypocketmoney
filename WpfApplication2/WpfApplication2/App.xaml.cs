using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using WpfApplication2.Repos;
using WpfApplication2.Services;
using WpfApplication2.ViewModels;
using WpfApplication2.utils;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BaseViewModel.Kernel.Bind<IRecordsRepository>().To<RecordsRepository>();
            BaseViewModel.Kernel.Bind<IDispatcher>().To<MyDispatcher>();
            BaseViewModel.Kernel.Bind<IMediator>().To<MessageMediator>();
            BaseViewModel.Kernel.Bind<IRegionManager>().To<RegionManager>();

            var regionManager = BaseViewModel.Kernel.Get<IRegionManager>();

            var window = regionManager.FindWindowByViewModel<MainViewModel>();
            window.Show();

        }
    }
}
