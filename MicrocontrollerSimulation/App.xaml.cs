using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MicrocontrollerSimulation.ViewModels.Microcontroller;

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
                services.AddSingleton<FunctionsCollection>();

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
                services.AddTransient<CreateNotFunctionViewModel>();
                services.AddSingleton<CreateMultiFunctionViewModel<And>>();
                services.AddTransient<CreateMultiFunctionViewModel<Or>>();
                services.AddTransient<CreateMultiFunctionViewModel<Xor>>();
                services.AddTransient<CreateFinalFunctionViewModel>();

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

            AddCustomFunction();
        }

        private void SetupNavigationState()
        {
            var mainNavigationStore = _host.Services.GetRequiredService<NavigationStore<MainWindowViewModel>>();
            mainNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<MainViewModel>();

            var functionsNavigationStore = _host.Services.GetRequiredService<NavigationStore<FunctionsSetupViewModel>>();
            functionsNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<FunctionsOverviewViewModel>();
        }

        private void AddCustomFunction()
        {
            Input a = new("A");
            Input b = new("B");
            Input c = new("C");

            And and1 = new(a, b);
            And and2 = new(new Not(a), c);
            And and3 = new(a, b);

            Or or1 = new(and1, c);
            Or or2 = new(and2, a);

            Xor xor = new(or1, and3);

            And and = new(xor, or2);

            var customExpression = new CustomExpression(and);

            var customFunction = new Function("Test_function", customExpression);

            _host.Services.GetRequiredService<FunctionsCollection>().Add(customFunction);
        }
    }
}
