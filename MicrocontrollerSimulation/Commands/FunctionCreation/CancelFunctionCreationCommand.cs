using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class CancelFunctionCreationCommand : NavigateCommand
    {
        private readonly CreateFunctionViewModel _createFunctionViewModel;

        public CancelFunctionCreationCommand(CreateFunctionViewModel createFunctionViewModel, INavigationService navigationService) : base(navigationService)
        {
            _createFunctionViewModel = createFunctionViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_createFunctionViewModel.Functions.Count == 0)
            {
                base.Execute(parameter);
                return;
            }

            if (MessageBox.Show("Veškerý postup ve vytváření funkce bude ztracen. Opravdu chcete odejít?", "Potvrzení", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                base.Execute(parameter);
            }
        }
    }
}
