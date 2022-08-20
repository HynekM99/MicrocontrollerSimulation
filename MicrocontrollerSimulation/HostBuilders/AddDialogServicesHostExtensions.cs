using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.Views.Windows.Simulation;
using MicrocontrollerSimulation.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrocontrollerSimulation.ViewModels.Projects;
using MicrocontrollerSimulation.ViewModels.Simulation;
using MicrocontrollerSimulation.ViewModels.Functions.Editing;

namespace MicrocontrollerSimulation.HostBuilders
{
    public static class AddDialogServicesHostExtensions
    {
        public static IHostBuilder AddDialogServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<DialogService<FunctionEditWindow>>();
                services.AddSingleton<DialogService<SelectProjectWindow>>();
                services.AddSingleton<DialogService<NewProjectWindow>>();
                services.AddSingleton<DialogService<AboutAppWindow>>();
                services.AddSingleton<DialogService<SimulationWindow>>();

                services.AddSingleton<FunctionEditDialogService>();

                services.AddSingleton<MenuDialogServices>();

                services.AddSingleton<Func<FunctionEditWindow>>(s => () => s.GetRequiredService<FunctionEditWindow>());
                services.AddSingleton<Func<SelectProjectWindow>>(s => () => s.GetRequiredService<SelectProjectWindow>());
                services.AddSingleton<Func<NewProjectWindow>>(s => () => s.GetRequiredService<NewProjectWindow>());
                services.AddSingleton<Func<AboutAppWindow>>(s => () => s.GetRequiredService<AboutAppWindow>());
                services.AddSingleton<Func<SimulationWindow>>(s => () => s.GetRequiredService<SimulationWindow>());

                services.AddSingleton<AboutAppWindow>();
                services.AddTransient(s => new FunctionEditWindow { DataContext = s.GetRequiredService<FunctionEditViewModel>() });
                services.AddTransient(s => new SimulationWindow { DataContext = s.GetRequiredService<SimulationViewModel>() });
                services.AddTransient(s => new NewProjectWindow { DataContext = s.GetRequiredService<NewProjectViewModel>() });
                services.AddTransient(s => new SelectProjectWindow { DataContext = s.GetRequiredService<SelectProjectViewModel>() });
            });

            return hostBuilder;
        }
    }
}
