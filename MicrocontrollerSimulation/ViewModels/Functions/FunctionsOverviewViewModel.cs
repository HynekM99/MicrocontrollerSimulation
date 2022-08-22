using MicrocontrollerSimulation.Commands;
using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Commands.FunctionEditing;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class FunctionsOverviewViewModel : ViewModelBase
    {
        private Function? _selectedFunction;
        public Function? SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                _selectedFunction = value;
                OnPropertyChanged(nameof(SelectedFunction));
                
                if (ShowTruthTableCommand is ShowTruthTableAsyncCommand command)
                {
                    command.CancelExecute();
                }

                if (SelectedFunction is not null && 
                    SelectedFunction.Expression.Inputs.Count < 13)
                {
                    ShowTruthTableCommand.Execute(null);
                }
                else
                {
                    TruthTable = null;
                }
            }
        }

        public FunctionsCollection Functions { get; }

        public bool ShowTableButtonVisible { get { return TruthTable is null; } }

        private DataTable? _truthTable;
        public DataTable? TruthTable
        {
            get { return _truthTable; }
            set
            {
                _truthTable?.Dispose();
                _truthTable = value;
                OnPropertyChanged(nameof(TruthTable));
                OnPropertyChanged(nameof(ShowTableButtonVisible));
            }
        }

        public ICommand ShowTruthTableCommand { get; }
        public ICommand NavigateToFunctionParserCommand { get; }
        public ICommand NavigateToFunctionCreationCommand { get; }
        public ICommand OpenFunctionEditDialogCommand { get; }
        public ICommand RemoveFunctionCommand { get; }

        public FunctionsOverviewViewModel(
            FunctionsCollection functions,
            FunctionEditDialogService functionEditDialogService,
            NavigationService<FunctionsSetupViewModel, ParseFunctionViewModel> functionParserNavigationService,
            NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel> createFunctionNavigationService)
        {
            Functions = functions;

            ShowTruthTableCommand = new ShowTruthTableAsyncCommand(this);
            NavigateToFunctionParserCommand = new NavigateCommand(functionParserNavigationService);
            NavigateToFunctionCreationCommand = new NavigateCommand(createFunctionNavigationService);
            OpenFunctionEditDialogCommand = new OpenFunctionEditDialogCommand(this, functionEditDialogService);
            RemoveFunctionCommand = new RemoveFunctionCommand(this, functions);

            Functions.FunctionChanged += OnFunctionChanged;
        }

        private void OnFunctionChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedFunction));

            if (SelectedFunction is not null &&
                    SelectedFunction.Expression.Inputs.Count < 13)
            {
                ShowTruthTableCommand.Execute(null);
            }
        }

        public override void Dispose()
        {
            if (ShowTruthTableCommand is ShowTruthTableAsyncCommand command)
            {
                command.CancelExecute();
            }
            Functions.FunctionChanged -= OnFunctionChanged;
            TruthTable?.Dispose();
            TruthTable = null;
        }
    }
}
