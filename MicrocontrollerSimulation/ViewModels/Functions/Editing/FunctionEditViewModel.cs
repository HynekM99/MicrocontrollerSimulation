using MicrocontrollerSimulation.Commands.FunctionEditing;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions.Editing
{
    public class FunctionEditViewModel : ViewModelBase
    {
        private Function? _function;
        public Function? Function
        {
            get { return _function; }
            set
            {
                _function = value;
                OnPropertyChanged(nameof(Function));
                Initialize();
            }
        }

        private string? _newName;
        public string? NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
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

        private string? _inputsErrorMessage;
        public string? InputsErrorMessage
        {
            get { return _inputsErrorMessage; }
            set
            {
                _inputsErrorMessage = value;
                OnPropertyChanged(nameof(InputsErrorMessage));
            }
        }

        private InputEditViewModel[]? _inputEditViewModels;
        public InputEditViewModel[]? InputEditViewModels
        {
            get { return _inputEditViewModels; }
            set
            {
                _inputEditViewModels = value;
                OnPropertyChanged(nameof(InputEditViewModels));
            }
        }

        public ICommand ConfirmEditCommand { get; }

        public FunctionEditViewModel(FunctionsCollection functions)
        {
            ConfirmEditCommand = new EditFunctionCommand(this, functions);
        }

        private void Initialize()
        {
            NewName = _function!.Name;

            var inputs = _function!.Expression.Inputs.ToList();
            InputEditViewModel[]? inputEditViewModels = new InputEditViewModel[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
            {
                inputEditViewModels[i] = new(inputs[i], _function!.Expression.Inputs);
                inputEditViewModels[i].PropertyChanged += OnInputEditViewModelPropertyChanged;
            }

            InputEditViewModels = inputEditViewModels;
        }

        private void OnInputEditViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(InputEditViewModels));
        }

        public override void Dispose()
        {
            if (InputEditViewModels is not null)
            {
                foreach (var inputVM in InputEditViewModels)
                {
                    inputVM.Dispose();
                    inputVM.PropertyChanged -= OnInputEditViewModelPropertyChanged;
                }
            }
            base.Dispose();
        }
    }
}
