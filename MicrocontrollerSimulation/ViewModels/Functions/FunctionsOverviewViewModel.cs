using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class FunctionsOverviewViewModel : ViewModelBase
    {
        public ObservableCollection<string> Functions { get; } = new();

        public ICommand CreateFunctionCommand { get; }

        public FunctionsOverviewViewModel(
            NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel> createFunctionNavigationService)
        {
            Functions.Add("funkce1");
            Functions.Add("funkce2");
            Functions.Add("funkce3");

            CreateFunctionCommand = new NavigateCommand(createFunctionNavigationService);
        }
    }
}
