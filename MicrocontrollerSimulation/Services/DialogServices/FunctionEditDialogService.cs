using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions.Editing;
using MicrocontrollerSimulation.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.DialogServices
{
    public class FunctionEditDialogService : DialogService<FunctionEditWindow>
    {
        public FunctionEditDialogService(Func<FunctionEditWindow> createWindow) : base(createWindow)
        {
        }

        public void ShowDialog(Function function)
        {
            var window = _createWindow();

            if (window.DataContext is FunctionEditViewModel feVM)
            {
                feVM.Function = function;
            }

            window.ShowDialog();

            if (window.DataContext is ViewModelBase vm)
            {
                vm.Dispose();
            }
        }
    }
}
