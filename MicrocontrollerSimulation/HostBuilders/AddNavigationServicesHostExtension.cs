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

                services.AddTransient<NavigationService<MainWindowViewModel, MainViewModel>>();

                services.AddTransient<NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel>>();
                services.AddTransient<NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel>>();
                services.AddTransient<NavigationService<FunctionsSetupViewModel, ParseFunctionViewModel>>();

                services.AddTransient<NavigationService<MicrocontrollerSetupViewModel, PinsOverviewViewModel>>();

                services.AddTransient<Func<MainViewModel>>(s => () => s.GetRequiredService<MainViewModel>());
                services.AddTransient<Func<FunctionsOverviewViewModel>>(s => () => s.GetRequiredService<FunctionsOverviewViewModel>());
                services.AddTransient<Func<CreateFunctionViewModel>>(s => () => s.GetRequiredService<CreateFunctionViewModel>());
                services.AddTransient<Func<ParseFunctionViewModel>>(s => () => s.GetRequiredService<ParseFunctionViewModel>());
                services.AddTransient<Func<PinsOverviewViewModel>>(s => () => s.GetRequiredService<PinsOverviewViewModel>());
                services.AddTransient<Func<SelectedPinConfigurationViewModel>>(s => () => s.GetRequiredService<SelectedPinConfigurationViewModel>());

                services.AddTransient<NavigationInitializerService>();
            });
            
            return hostBuilder;
        }
    }
}
