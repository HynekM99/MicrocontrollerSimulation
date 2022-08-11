using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
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
    public class CreateFinalFunctionViewModel : ViewModelBase
    {
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

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

        private FunctionsCollection? _temporaryFunctions;
        public FunctionsCollection? TemporaryFunctions
        {
            get { return _temporaryFunctions; }
            set
            {
                _temporaryFunctions = value;
                OnPropertyChanged(nameof(TemporaryFunctions));
            }
        }

        public FunctionsCollection Functions { get; }

        public ICommand CreateFinalFunctionCommand { get; }

        public CreateFinalFunctionViewModel(
            FunctionsCollection functions,
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> navigationService)
        {
            Functions = functions;

            CreateFinalFunctionCommand = new CreateCustomFunctionCommand(this, navigationService);
        }
    }
}
