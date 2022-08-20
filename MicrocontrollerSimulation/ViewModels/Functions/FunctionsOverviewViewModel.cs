using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            }
        }

        public FunctionsCollection Functions { get; }

        public ICommand NavigateToFunctionCreationCommand { get; }
        public ICommand OpenFunctionEditDialogCommand { get; }
        public ICommand RemoveFunctionCommand { get; }

        public FunctionsOverviewViewModel(
            FunctionsCollection functions,
            FunctionEditDialogService functionEditDialogService,
            NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel> createFunctionNavigationService)
        {
            Functions = functions;

            NavigateToFunctionCreationCommand = new NavigateCommand(createFunctionNavigationService);
            OpenFunctionEditDialogCommand = new RelayCommand(e => functionEditDialogService.ShowDialog(SelectedFunction!));
            RemoveFunctionCommand = new RemoveFunctionCommand(this, functions);

            Functions.FunctionChanged += OnFunctionChanged;
        }

        private void OnFunctionChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedFunction));
        }

        public override void Dispose()
        {
            Functions.FunctionChanged -= OnFunctionChanged;
            base.Dispose();
        }
    }
}
