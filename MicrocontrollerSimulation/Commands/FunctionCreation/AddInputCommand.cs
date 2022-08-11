using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class AddInputCommand : CommandBase
    {
        private FunctionsCollection _functions;
        private CreateFunctionViewModel _createFunctionViewModel;

        public AddInputCommand(CreateFunctionViewModel createFunctionViewModel)
        {
            _functions = createFunctionViewModel.Functions;
            _createFunctionViewModel = createFunctionViewModel;
            _createFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            string name = _createFunctionViewModel.NewInputName!;

            _functions.Insert(0, new Function(name, new Input(name)));
            _createFunctionViewModel.NewInputName = null;
        }

        public override bool CanExecute(object? parameter)
        {
            string? name = _createFunctionViewModel.NewInputName;

            if (string.IsNullOrWhiteSpace(name))
            {
                _createFunctionViewModel.ErrorMessage = null;
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _createFunctionViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                return false;
            }

            if (_functions.Any(f => f.Name == name))
            {
                _createFunctionViewModel.ErrorMessage = "Vstup s tímto názvem již existuje.";
                return false;
            }

            _createFunctionViewModel.ErrorMessage = null;
            return base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_createFunctionViewModel.NewInputName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
