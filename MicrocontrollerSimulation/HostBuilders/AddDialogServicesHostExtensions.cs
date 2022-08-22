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
                services.AddTransient<DialogService<FunctionEditWindow>>();
                services.AddTransient<DialogService<SelectProjectWindow>>();
                services.AddTransient<DialogService<NewProjectWindow>>();
                services.AddTransient<DialogService<AboutAppWindow>>();
                services.AddTransient<DialogService<SimulationWindow>>();

                services.AddTransient<FunctionEditDialogService>();

                services.AddTransient<MenuDialogServices>();

                services.AddTransient<Func<FunctionEditWindow>>(s => () => s.GetRequiredService<FunctionEditWindow>());
                services.AddTransient<Func<SelectProjectWindow>>(s => () => s.GetRequiredService<SelectProjectWindow>());
                services.AddTransient<Func<NewProjectWindow>>(s => () => s.GetRequiredService<NewProjectWindow>());
                services.AddTransient<Func<AboutAppWindow>>(s => () => s.GetRequiredService<AboutAppWindow>());
                services.AddTransient<Func<SimulationWindow>>(s => () => s.GetRequiredService<SimulationWindow>());

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
