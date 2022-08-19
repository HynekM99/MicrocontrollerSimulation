using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Services.NavigationServices;
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
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Services.SavingServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.HostBuilders;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.ViewModels.Projects;
using MicrocontrollerSimulation.Views.Windows;
using MicrocontrollerSimulation.Views.Windows.Simulation;
using MicrocontrollerSimulation.ViewModels.Simulation;

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
                            s.GetRequiredService<IProjectToJsonService>());
                    });
                    services.AddSingleton<ILoadingService, JsonLoadingService>();

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

                    services.AddTransient<SimulationViewModel>();

                    services.AddTransient(s =>
                    {
                        return new NewProjectViewModel(
                            PROJECTS_DIRECTORY,
                            s.GetRequiredService<CurrentProject>(),
                            s.GetRequiredService<NavigationInitializerService>());
                    });

                    services.AddTransient(s =>
                    {
                        return new SelectProjectViewModel(
                            PROJECTS_DIRECTORY,
                            s.GetRequiredService<CurrentProject>(),
                            s.GetRequiredService<ILoadingService>(),
                            s.GetRequiredService<NavigationInitializerService>());
                    });
                    
                    services.AddSingleton<DialogService<SelectProjectWindow>>();
                    services.AddSingleton<DialogService<NewProjectWindow>>();
                    services.AddSingleton<DialogService<AboutAppWindow>>();
                    services.AddSingleton<DialogService<SimulationWindow>>();

                    services.AddSingleton<MenuDialogServices>();

                    services.AddSingleton<Func<SelectProjectWindow>>(s => () => s.GetRequiredService<SelectProjectWindow>());
                    services.AddSingleton<Func<NewProjectWindow>>(s => () => s.GetRequiredService<NewProjectWindow>());
                    services.AddSingleton<Func<AboutAppWindow>>(s => () => s.GetRequiredService<AboutAppWindow>());
                    services.AddSingleton<Func<SimulationWindow>>(s => () => s.GetRequiredService<SimulationWindow>());

                    services.AddSingleton<AboutAppWindow>();
                    services.AddTransient(s => new SimulationWindow { DataContext = s.GetRequiredService<SimulationViewModel>() });
                    services.AddTransient(s => new NewProjectWindow { DataContext = s.GetRequiredService<NewProjectViewModel>() });
                    services.AddTransient(s => new SelectProjectWindow { DataContext = s.GetRequiredService<SelectProjectViewModel>() });
                    services.AddSingleton(s => new MainWindow { DataContext = s.GetRequiredService<MainWindowViewModel>() });
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

            Exit += OnAppExit;
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            var project = _host.Services.GetRequiredService<CurrentProject>();
            project.Save();
        }
    }
}
