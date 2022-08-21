using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Services.ExpressionParsingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class ParseFunctionViewModel : ViewModelBase
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

        private string? _expression;
        public string? Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;
                OnPropertyChanged(nameof(Expression));
            }
        }

        private string? _parseErrorMessage;
        public string? ParseErrorMessage
        {
            get { return _parseErrorMessage; }
            set
            {
                _parseErrorMessage = value;
                OnPropertyChanged(nameof(ParseErrorMessage));
            }
        }

        public ICommand CreateFunctionCommand { get; }
        public ICommand CancelCommand { get; }

        public ParseFunctionViewModel(
            FunctionsCollection functions, 
            IExpressionParser expressionParser, 
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsOverviewNavigationService)
        {
            CreateFunctionCommand = new ParseFunctionCommand(this, functions, expressionParser, functionsOverviewNavigationService);

            CancelCommand = new RelayCommand(e =>
            {
                if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrEmpty(Expression))
                {
                    functionsOverviewNavigationService.Navigate();
                    return;
                }

                if (MessageBox.Show("Veškerý postup ve vytváření funkce bude ztracen. Opravdu chcete odejít?", "Potvrzení", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    functionsOverviewNavigationService.Navigate();
                }
            });
        }
    }
}
