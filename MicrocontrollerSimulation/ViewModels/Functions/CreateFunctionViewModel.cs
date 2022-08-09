using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
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
    public class CreateFunctionViewModel : ViewModelBase
    {
        private string? _newInputName;
        public string? NewInputName
        {
            get { return _newInputName; }
            set
            {
                _newInputName = value;
                OnPropertyChanged(nameof(NewInputName));
            }
        }

        private string? _inputNameErrorMessage;
        public string? InputNameErrorMessage
        {
            get { return _inputNameErrorMessage; }
            set
            {
                _inputNameErrorMessage = value;
                OnPropertyChanged(nameof(InputNameErrorMessage));
            }
        }

        public Function? SelectedForNotFunction { get; set; }

        public FunctionsCollection Functions { get; } = new();

        public ICommand CancelCommand { get; }
        public ICommand AddInputCommand { get; }
        public ICommand AddNotFunctionCommand { get; }

        public CreateFunctionViewModel(
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsOverviewNavigationService)
        {
            CancelCommand = new CancelFunctionCreationCommand(this, functionsOverviewNavigationService);
            AddInputCommand = new AddInputCommand(this);
            AddNotFunctionCommand = new RelayCommand(e =>
            {
                var not = new Not(SelectedForNotFunction.Expression);
                Functions.Add(new(not.AsString, not));
            });
        }
    }
}
