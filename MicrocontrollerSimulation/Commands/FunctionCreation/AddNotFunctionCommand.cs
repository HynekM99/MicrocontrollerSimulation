using MicrocontrollerSimulation.Commands.Base;
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
    public class AddNotFunctionCommand : CommandBase
    {
        private CreateNotFunctionViewModel _createNotFunctionViewModel;

        public AddNotFunctionCommand(CreateNotFunctionViewModel createNotFunctionViewModel)
        {
            _createNotFunctionViewModel = createNotFunctionViewModel;

            _createNotFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            var functions = _createNotFunctionViewModel.Functions!;
            var selected = _createNotFunctionViewModel.SelectedFunction!;
            var not = new Not(selected.Expression);

            functions.Insert(0, new(not.AsString, not));

            _createNotFunctionViewModel.SelectedFunction = null;
        }

        public override bool CanExecute(object? parameter)
        {
            return _createNotFunctionViewModel.Functions is not null &&
                _createNotFunctionViewModel.SelectedFunction is not null && 
                base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_createNotFunctionViewModel.SelectedFunction))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
