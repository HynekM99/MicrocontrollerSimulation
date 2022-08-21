using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class RemoveFunctionCommand : CommandBase
    {
        private readonly FunctionsOverviewViewModel _functionsOverviewViewModel;
        private readonly FunctionsCollection _functions;

        private readonly string[] _unremovableFunctions = new string[] { "Not", "And", "Or", "Xor", "Nand", "Nor", "Xnor", "RS_Latch" };

        public RemoveFunctionCommand(FunctionsOverviewViewModel functionsOverviewViewModel, FunctionsCollection functions)
        {
            _functionsOverviewViewModel = functionsOverviewViewModel;
            _functions = functions;

            _functionsOverviewViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_functionsOverviewViewModel.SelectedFunction))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            var selectedFunction = _functionsOverviewViewModel.SelectedFunction!;
            return selectedFunction is not null &&
                   !_unremovableFunctions.Contains(selectedFunction.Name) &&
                   base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            var selectedFunction = _functionsOverviewViewModel.SelectedFunction!;
            if (MessageBox.Show($"Opravdu chcete smazat funkci \"{selectedFunction!.Name}\"", "Potvrzení", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                _functions.Remove(selectedFunction);
            }
        }
    }
}
