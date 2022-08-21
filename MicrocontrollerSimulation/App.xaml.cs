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
using System.IO;

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
            _host = Host.CreateDefaultBuilder()
                .AddNavigationServices()
                .AddDialogServices()
                .AddViewModels()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<CurrentProject>();
                    services.AddTransient(s => s.GetRequiredService<CurrentProject>().ProjectInfo.Functions);
                    services.AddTransient(s => s.GetRequiredService<CurrentProject>().ProjectInfo.Microcontroller);

                    services.AddSingleton<IDeviceFactory, BasicDeviceFactory>();

                    services.AddTransient<IProjectToJsonService, ProjectToJsonService>();
                    services.AddSingleton<IJsonToProjectService, JsonToProjectService>();

                    services.AddTransient(CreateSavingService);
                    services.AddSingleton<ILoadingService, JsonLoadingService>();

                    services.AddSingleton(s => new MainWindow { DataContext = s.GetRequiredService<MainWindowViewModel>() });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            SetDefaultProject();

            _host.Services.GetRequiredService<NavigationInitializerService>().Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void SetDefaultProject()
        {
            ProjectInfo? project;

            var loadingService = _host.Services.GetRequiredService<ILoadingService>();
            var path = Path.Combine(
                ProjectInfo.DEFAULT_PROJECT_DIRECTORY,
                $"{ProjectInfo.DEFAULT_PROJECT_NAME}.json");

            try { project = loadingService.Load(path); }
            catch { project = null; }

            if (project is not null)
            {
                _host.Services.GetRequiredService<CurrentProject>().ProjectInfo = project;
            }
        }
        private ISavingService CreateSavingService(IServiceProvider services)
        {
            return new JsonFileSavingService(
                ProjectInfo.DEFAULT_PROJECT_DIRECTORY,
                services.GetRequiredService<IProjectToJsonService>());
        }
    }
}
