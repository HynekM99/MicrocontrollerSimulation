using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using MicrocontrollerSimulation.ViewModels.Simulation;
using MicrocontrollerSimulation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Projects;

namespace MicrocontrollerSimulation.HostBuilders
{
    public static class AddViewModelsServiceHostExtension
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
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

                services.AddTransient(CreateNewProjectViewModel);
                services.AddTransient(CreateSelectProjectViewModel);
            });

            return hostBuilder;
        }

        private static NewProjectViewModel CreateNewProjectViewModel(IServiceProvider services)
        {
            return new NewProjectViewModel(
                ProjectInfo.DEFAULT_PROJECT_DIRECTORY,
                services.GetRequiredService<CurrentProject>(),
                services.GetRequiredService<NavigationInitializerService>());
        }

        private static SelectProjectViewModel CreateSelectProjectViewModel(IServiceProvider services)
        {
            return new SelectProjectViewModel(
                ProjectInfo.DEFAULT_PROJECT_DIRECTORY,
                services.GetRequiredService<CurrentProject>(),
                services.GetRequiredService<ILoadingService>(),
                services.GetRequiredService<NavigationInitializerService>());
        }
    }
}
