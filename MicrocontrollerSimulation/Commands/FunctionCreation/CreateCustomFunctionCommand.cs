using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class CreateCustomFunctionCommand : NavigateCommand
    {
        private readonly CreateFinalFunctionViewModel _selectFinalFunctionViewModel;
        private readonly NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> _navigationService;
        private readonly FunctionsCollection _functions;

        public CreateCustomFunctionCommand(
            CreateFinalFunctionViewModel selectFinalFunctionViewModel,
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> navigationService) : base(navigationService)
        {
            _selectFinalFunctionViewModel = selectFinalFunctionViewModel;
            _navigationService = navigationService;
            _functions = _selectFinalFunctionViewModel.Functions;

            _selectFinalFunctionViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            var selected = _selectFinalFunctionViewModel.SelectedFunctionViewModel!.Function;
            var custom = new CustomExpression(selected.Expression);
            var name = _selectFinalFunctionViewModel.Name!;

            _functions.Add(new(name, custom));

            _navigationService.Navigate();
        }

        public override bool CanExecute(object? parameter)
        {
            string? name = _selectFinalFunctionViewModel.Name;

            if (string.IsNullOrEmpty(name))
            {
                _selectFinalFunctionViewModel.ErrorMessage = null;
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _selectFinalFunctionViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                return false;
            }

            if (_functions.Any(f => f.Name == name))
            {
                _selectFinalFunctionViewModel.ErrorMessage = "Funkce s tímto názvem již existuje.";
                return false;
            }

            _selectFinalFunctionViewModel.ErrorMessage = null;

            return _selectFinalFunctionViewModel.Functions is not null &&
                _selectFinalFunctionViewModel.SelectedFunctionViewModel is not null &&
                base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_selectFinalFunctionViewModel.SelectedFunctionViewModel) ||
                e.PropertyName == nameof(_selectFinalFunctionViewModel.Name))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
