using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionEditing
{
    public class OpenFunctionEditDialogCommand : CommandBase
    {
        private readonly string[] _unremovableFunctions = new string[] { "Not", "And", "Or", "Xor", "Nand", "Nor", "Xnor", "RS_Latch" };

        private readonly FunctionsOverviewViewModel _functionsOverviewViewModel;
        private readonly FunctionEditDialogService _functionEditDialogService;

        public OpenFunctionEditDialogCommand(FunctionsOverviewViewModel functionsOverviewViewModel, FunctionEditDialogService functionEditDialogService)
        {
            _functionsOverviewViewModel = functionsOverviewViewModel;
            _functionEditDialogService = functionEditDialogService;

            _functionsOverviewViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            var selectedFunction = _functionsOverviewViewModel.SelectedFunction;
            return selectedFunction is not null &&
                !_unremovableFunctions.Contains(selectedFunction.Name) &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            var function = _functionsOverviewViewModel.SelectedFunction!;
            _functionEditDialogService.ShowDialog(function);
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_functionsOverviewViewModel.SelectedFunction))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
