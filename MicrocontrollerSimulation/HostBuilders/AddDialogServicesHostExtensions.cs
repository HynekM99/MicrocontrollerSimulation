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

namespace MicrocontrollerSimulation.HostBuilders
{
    public static class AddDialogServicesHostExtensions
    {
        public static IHostBuilder AddDialogServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<DialogService<SelectProjectWindow>>();
                services.AddSingleton<DialogService<NewProjectWindow>>();
                services.AddSingleton<DialogService<AboutAppWindow>>();
                services.AddSingleton<DialogService<SimulationWindow>>();

                services.AddSingleton<MenuDialogServices>();

                services.AddSingleton<Func<SelectProjectWindow>>(s => () => s.GetRequiredService<SelectProjectWindow>());
                services.AddSingleton<Func<NewProjectWindow>>(s => () => s.GetRequiredService<NewProjectWindow>());
                services.AddSingleton<Func<AboutAppWindow>>(s => () => s.GetRequiredService<AboutAppWindow>());
                services.AddSingleton<Func<SimulationWindow>>(s => () => s.GetRequiredService<SimulationWindow>());
            });

            return hostBuilder;
        }
    }
}
