using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore<MainWindowViewModel>>();
                services.AddSingleton<NavigationStore<FunctionsSetupViewModel>>();

                services.AddSingleton<NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel>>();
                services.AddSingleton<NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel>>();

                services.AddSingleton<Func<FunctionsOverviewViewModel>>(s => () => s.GetRequiredService<FunctionsOverviewViewModel>());
                services.AddSingleton<Func<CreateFunctionViewModel>>(s => () => s.GetRequiredService<CreateFunctionViewModel>());

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<MicrocontrollerSetupViewModel>();

                services.AddSingleton<FunctionsSetupViewModel>();
                services.AddTransient<FunctionsOverviewViewModel>();
                services.AddTransient<CreateFunctionViewModel>();

                services.AddSingleton(s => new MainWindow() { DataContext = s.GetRequiredService<MainWindowViewModel>() });
            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            SetupNavigationState();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void SetupNavigationState()
        {
            var mainNavigationStore = _host.Services.GetRequiredService<NavigationStore<MainWindowViewModel>>();
            mainNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<MainViewModel>();

            var functionsNavigationStore = _host.Services.GetRequiredService<NavigationStore<FunctionsSetupViewModel>>();
            functionsNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<FunctionsOverviewViewModel>();
        }
    }
}
