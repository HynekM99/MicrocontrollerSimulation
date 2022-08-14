using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class FunctionsOverviewViewModel : ViewModelBase
    {
        

        private Function? _selectedFunction;
        public Function? SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                _selectedFunction = value;
                OnPropertyChanged(nameof(SelectedFunction));
            }
        }

        public FunctionsCollection Functions { get; }

        public ICommand NavigateToFunctionCreationCommand { get; }
        public ICommand RemoveFunctionCommand { get; }

        public FunctionsOverviewViewModel(
            FunctionsCollection functions,
            NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel> createFunctionNavigationService)
        {
            Functions = functions;

            NavigateToFunctionCreationCommand = new NavigateCommand(createFunctionNavigationService);
            RemoveFunctionCommand = new RemoveFunctionCommand(this, functions);
        }
    }
}
