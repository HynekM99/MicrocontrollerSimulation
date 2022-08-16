using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.HostBuilders
{
    public static class AddNavigationServicesHostExtension
    {
        public static IHostBuilder AddNavigationServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore<MainWindowViewModel>>();
                services.AddSingleton<NavigationStore<FunctionsSetupViewModel>>();
                services.AddSingleton<NavigationStore<MicrocontrollerSetupViewModel>>();

                services.AddSingleton<NavigationService<MainWindowViewModel, MainViewModel>>();

                services.AddSingleton<NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel>>();
                services.AddSingleton<NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel>>();

                services.AddSingleton<NavigationService<MicrocontrollerSetupViewModel, PinsOverviewViewModel>>();

                services.AddSingleton<Func<MainViewModel>>(s => () => s.GetRequiredService<MainViewModel>());
                services.AddSingleton<Func<FunctionsOverviewViewModel>>(s => () => s.GetRequiredService<FunctionsOverviewViewModel>());
                services.AddSingleton<Func<CreateFunctionViewModel>>(s => () => s.GetRequiredService<CreateFunctionViewModel>());
                services.AddSingleton<Func<PinsOverviewViewModel>>(s => () => s.GetRequiredService<PinsOverviewViewModel>());
                services.AddSingleton<Func<SelectedPinConfigurationViewModel>>(s => () => s.GetRequiredService<SelectedPinConfigurationViewModel>());

                services.AddSingleton<NavigationInitializerService>();
            });

            return hostBuilder;
        }
    }
}
