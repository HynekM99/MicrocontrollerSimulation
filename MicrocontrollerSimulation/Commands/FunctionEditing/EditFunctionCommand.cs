using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.ViewModels.Functions.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionEditing
{
    public class EditFunctionCommand : CommandBase
    {
        private readonly FunctionEditViewModel _functionEditViewModel;
        private readonly FunctionsCollection _functions;

        public EditFunctionCommand(FunctionEditViewModel functionEditViewModel, FunctionsCollection functions)
        {
            _functionEditViewModel = functionEditViewModel;
            _functions = functions;

            _functionEditViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_functionEditViewModel.NewName))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            var name = _functionEditViewModel.NewName;

            if (string.IsNullOrEmpty(name))
            {
                _functionEditViewModel.ErrorMessage = null;
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _functionEditViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                return false;
            }

            if (_functions.Any(f => f.Name == name && f != _functionEditViewModel.Function))
            {
                _functionEditViewModel.ErrorMessage = "Funkce s tímto názvem již existuje.";
                return false;
            }

            _functionEditViewModel.ErrorMessage = null;
            return base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _functionEditViewModel.Function!.Name = _functionEditViewModel.NewName!;
        }
    }
}
