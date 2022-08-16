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
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Services.SavingServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.HostBuilders;
using MicrocontrollerSimulation.Services.LoadingServices;

namespace MicrocontrollerSimulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string PROJECTS_DIRECTORY = @".\projects\";

        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddNavigationServices()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<CurrentProject>();
                    services.AddTransient(s => s.GetRequiredService<CurrentProject>().ProjectInfo.Functions);
                    services.AddTransient(s => s.GetRequiredService<CurrentProject>().ProjectInfo.Microcontroller);

                    services.AddSingleton<IDeviceFactory, BasicDeviceFactory>();

                    services.AddTransient<IProjectToJsonService, ProjectToJsonService>();
                    services.AddSingleton<IJsonToProjectService, JsonToProjectService>();

                    services.AddTransient<ISavingService>(s =>
                    {
                        return new JsonFileSavingService(
                            PROJECTS_DIRECTORY,
                            s.GetRequiredService<CurrentProject>(),
                            s.GetRequiredService<IProjectToJsonService>());
                    });
                    services.AddSingleton<ILoadingService>(s =>
                    {
                        return new FileLoadingService(PROJECTS_DIRECTORY);
                    });

                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindowViewModel>();

                    services.AddTransient<FunctionsSetupViewModel>();
                    services.AddTransient<FunctionsOverviewViewModel>();

                    services.AddTransient<CreateFunctionViewModel>();
                    services.AddTransient<CreateNotFunctionViewModel>();
                    services.AddTransient<CreateMultiFunctionViewModel<And>>();
                    services.AddTransient<CreateMultiFunctionViewModel<Or>>();
                    services.AddTransient<CreateMultiFunctionViewModel<Xor>>();
                    services.AddTransient<CreateFinalFunctionViewModel>();

                    services.AddTransient<MicrocontrollerSetupViewModel>();
                    services.AddTransient<PinsOverviewViewModel>();

                    services.AddTransient<SelectedPinInputModeConfigViewModel>();
                    services.AddTransient<SelectedPinOutputModeConfigViewModel>();
                    services.AddTransient<SelectedPinConfigurationViewModel>();

                    services.AddSingleton(s => new MainWindow() { DataContext = s.GetRequiredService<MainWindowViewModel>() });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            _host.Services.GetRequiredService<NavigationInitializerService>().Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);

            AddCustomFunction();

            Exit += OnAppExit;
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

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            var savingService = _host.Services.GetRequiredService<ISavingService>();
            savingService.Save();
        }
    }
}
