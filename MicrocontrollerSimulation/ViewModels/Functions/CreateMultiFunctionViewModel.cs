using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class CreateMultiFunctionViewModel<T> : ViewModelBase where T : LogicalExpression
    {
        private FunctionsCollection? _selectedFunctions;
        public FunctionsCollection? SelectedFunctions
        {
            get { return _selectedFunctions; }
            set
            {
                _selectedFunctions = value;
                OnPropertyChanged(nameof(SelectedFunctions));
            }
        }

        private FunctionsCollection? _functions;
        public FunctionsCollection? Functions
        {
            get { return _functions; }
            set
            {
                _functions = value;
                OnPropertyChanged(nameof(Functions));
            }
        }

        private string? _functionPreview;
        public string? FunctionPreview
        {
            get { return _functionPreview; }
            set
            {
                _functionPreview = value;
                OnPropertyChanged(nameof(FunctionPreview));
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

        public ICommand AddMultiFunctionCommand { get; }
        public ICommand SelectionChangedCommand { get; } 

        public CreateMultiFunctionViewModel()
        {
            AddMultiFunctionCommand = new AddMultiFunctionCommand<T>(this);
            SelectionChangedCommand = new RelayCommand(UpdateSelectedFunctions);
        }

        private void UpdateSelectedFunctions(object? e)
        {
            var functions = (IList<object>?)e;

            if (functions is null)
            {
                SelectedFunctions = null;
                return;
            }

            var selectedFunctions = new FunctionsCollection();

            foreach (Function function in functions)
            {
                selectedFunctions.Add(function);
            }

            SelectedFunctions = selectedFunctions;
        }
    }
}
