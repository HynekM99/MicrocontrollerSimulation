using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation.Services.DialogServices
{
    public class DialogService<TWindow> : IDialogService where TWindow : Window
    {
        private readonly Func<TWindow> _createWindow;

        public DialogService(Func<TWindow> createWindow)
        {
            _createWindow = createWindow;
        }

        public void Show()
        {
            _createWindow().Show();
        }

        public void ShowDialog()
        {
            var window = _createWindow();
            window.ShowDialog();
            
            if (window.DataContext is ViewModelBase vm)
            {
                vm.Dispose();
            }
        }
    }
}
