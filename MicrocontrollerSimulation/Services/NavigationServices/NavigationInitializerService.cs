using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.NavigationServices
{
    public class NavigationInitializerService : INavigationService
    {
        private readonly NavigationService<MainWindowViewModel, MainViewModel> _mainNavigationService;
        private readonly NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> _functionsNavigationService;
        private readonly NavigationService<MicrocontrollerSetupViewModel, PinsOverviewViewModel> _microcontrollerNavigationService;

        public NavigationInitializerService(
            NavigationService<MainWindowViewModel, MainViewModel> mainNavigationService, 
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsNavigationService, 
            NavigationService<MicrocontrollerSetupViewModel, PinsOverviewViewModel> microcontrollerNavigationService)
        {
            _mainNavigationService = mainNavigationService;
            _functionsNavigationService = functionsNavigationService;
            _microcontrollerNavigationService = microcontrollerNavigationService;
        }

        public void Navigate()
        {
            _mainNavigationService.Navigate();
            _functionsNavigationService.Navigate();
            _microcontrollerNavigationService.Navigate();
        }
    }
}
