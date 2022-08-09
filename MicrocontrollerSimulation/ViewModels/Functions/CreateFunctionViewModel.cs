using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class CreateFunctionViewModel : ViewModelBase
    {
        public string Text { get; set; } = "Function creator";

        public ICommand CancelCommand { get; }

        public CreateFunctionViewModel(
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsOverviewNavigationService)
        {
            CancelCommand = new NavigateCommand(functionsOverviewNavigationService);
        }
    }
}
