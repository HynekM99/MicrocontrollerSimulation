using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Services.ExpressionParsingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class ParseFunctionCommand : CommandBase
    {
        private readonly ParseFunctionViewModel _parseFunctionViewModel;
        private readonly FunctionsCollection _functions;
        private readonly IExpressionParser _expressionParser;
        private readonly NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> _functionsOverviewNavigationService;

        public ParseFunctionCommand(
            ParseFunctionViewModel parseFunctionViewModel, 
            FunctionsCollection functions, 
            IExpressionParser expressionParser, 
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsOverviewNavigationService)
        {
            _parseFunctionViewModel = parseFunctionViewModel;
            _functions = functions;
            _expressionParser = expressionParser;
            _functionsOverviewNavigationService = functionsOverviewNavigationService;

            _parseFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            var name = _parseFunctionViewModel.Name;

            bool canExecute = true;

            _parseFunctionViewModel.ErrorMessage = null;
            _parseFunctionViewModel.ParseErrorMessage = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                _parseFunctionViewModel.ErrorMessage = null;
                canExecute = false;
            }

            if (!string.IsNullOrWhiteSpace(name) && !name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _parseFunctionViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                canExecute = false;
            }

            if (_functions.Any(f => f.Name == name))
            {
                _parseFunctionViewModel.ErrorMessage = "Funkce s tímto názvem již existuje.";
                canExecute = false;
            }

            var parsedExpression = TryParse();

            canExecute &= parsedExpression is not null;

            return canExecute && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            var name = _parseFunctionViewModel.Name;
            var parsedExpression = new CustomExpression(TryParse()!);

            var function = new Function(name!, parsedExpression);

            _functions.Add(function);

            _functionsOverviewNavigationService.Navigate();
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_parseFunctionViewModel.Name) ||
                e.PropertyName == nameof(_parseFunctionViewModel.Expression))
            {
                OnCanExecuteChanged();
            }
        }

        private LogicalExpression? TryParse()
        {
            var expression = _parseFunctionViewModel.Expression!;

            try
            {
                return _expressionParser.Parse(expression);
            } 
            catch (ArgumentException ex)
            {
                _parseFunctionViewModel.ParseErrorMessage = ex.Message;
            }

            return null;
        }
    }
}
