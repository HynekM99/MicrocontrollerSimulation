﻿using MicrocontrollerSimulation.Commands.Base;
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
    internal class AddMultiFunctionCommand<T> : CommandBase where T : LogicalExpression
    {
        private readonly CreateMultiFunctionViewModel<T> _createMultiFunctionViewModel;

        public AddMultiFunctionCommand(CreateMultiFunctionViewModel<T> createNotFunctionViewModel)
        {
            if (typeof(T).Name != nameof(And) &&
                typeof(T).Name != nameof(Or) &&
                typeof(T).Name != nameof(Xor))
            {
                throw new Exception($"The only allowed generic types are And, Or and Xor. \"{typeof(T).Name}\" provided.");
            }

            _createMultiFunctionViewModel = createNotFunctionViewModel;

            _createMultiFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            var functionVMs = _createMultiFunctionViewModel.SelectedFunctionViewModels!
                .Select(e => e.Function)
                .ToList();

            var expression = CreateExpression()!;

            var functions = _createMultiFunctionViewModel.TemporaryFunctions!;
            functions.Insert(0, new(expression!.AsString, expression));

            _createMultiFunctionViewModel.TemporaryFunctions = null;
            _createMultiFunctionViewModel.TemporaryFunctions = functions;
            _createMultiFunctionViewModel.SelectedFunctionViewModels = null;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_createMultiFunctionViewModel.TemporaryFunctions is null ||
                _createMultiFunctionViewModel.SelectedFunctionViewModels is null ||
                _createMultiFunctionViewModel.SelectedFunctionViewModels.Count < 2)
            {
                _createMultiFunctionViewModel.FunctionPreview = null;
                _createMultiFunctionViewModel.ErrorMessage = null;
                return false;
            }

            var expression = CreateExpression()!;

            _createMultiFunctionViewModel.FunctionPreview = expression.AsString;

            if (_createMultiFunctionViewModel.TemporaryFunctions.Any(f => f.Name == expression.AsString))
            {
                _createMultiFunctionViewModel.ErrorMessage = "Identická funkce již existuje.";
                return false;
            }

            _createMultiFunctionViewModel.ErrorMessage = null;

            return base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_createMultiFunctionViewModel.SelectedFunctionViewModels))
            {
                OnCanExecuteChanged();
            }
        }

        private LogicalExpression? CreateExpression()
        {
            var selectedFunctions = _createMultiFunctionViewModel.SelectedFunctionViewModels!
                .Select(e => e.Function);

            var selectedExpressions = selectedFunctions.ToList().ConvertAll(x => x.Expression);

            LogicalExpression? expression = null;

            if (typeof(T).Name == nameof(And))
            {
                expression = new And(selectedExpressions);
            }
            else if (typeof(T).Name == nameof(Or))
            {
                expression = new Or(selectedExpressions);
            }
            else if (typeof(T).Name == nameof(Xor))
            {
                expression = new Xor(selectedExpressions);
            }

            return expression;
        }
    }
}
