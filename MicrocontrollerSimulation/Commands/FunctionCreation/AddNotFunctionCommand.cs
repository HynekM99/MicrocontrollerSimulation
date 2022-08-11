using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class AddNotFunctionCommand : CommandBase
    {
        private readonly CreateNotFunctionViewModel _createNotFunctionViewModel;

        public AddNotFunctionCommand(CreateNotFunctionViewModel createNotFunctionViewModel)
        {
            _createNotFunctionViewModel = createNotFunctionViewModel;

            _createNotFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            var functions = _createNotFunctionViewModel.Functions!;
            var not = CreateNotExpression()!;

            functions.Insert(0, new(not.AsString, not));

            _createNotFunctionViewModel.SelectedFunction = null;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_createNotFunctionViewModel.SelectedFunction is null)
            {
                _createNotFunctionViewModel.ErrorMessage = null;
                return false;
            }

            var functions = _createNotFunctionViewModel.Functions!;
            var not = CreateNotExpression()!;

            if (functions.Any(f => f.Name == not.AsString))
            {
                _createNotFunctionViewModel.ErrorMessage = "Identická funkce již existuje.";
                return false;
            }

            _createNotFunctionViewModel.ErrorMessage = null;

            return _createNotFunctionViewModel.Functions is not null &&
                base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_createNotFunctionViewModel.SelectedFunction))
            {
                OnCanExecuteChanged();
            }
        }

        private Not? CreateNotExpression()
        {
            var selected = _createNotFunctionViewModel.SelectedFunction;
            return selected is null ? null : new Not(selected.Expression);
        }
    }
}
