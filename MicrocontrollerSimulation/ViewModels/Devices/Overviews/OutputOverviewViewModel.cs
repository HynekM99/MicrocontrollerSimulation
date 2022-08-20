using MicrocontrollerSimulation.Models.Functions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Overviews
{
    public class OutputOverviewViewModel : DeviceOverviewViewModel
    {
        public Function Function { get; }

        public OutputOverviewViewModel(Function function)
        {
            Function = function;

            Function.FunctionRenamed += OnFunctionRenamed;
        }

        private void OnFunctionRenamed(object? sender, Models.Functions.FunctionEventArgs.FunctionRenamedEventArgs e)
        {
            OnPropertyChanged(nameof(Function));
        }

        public override void Dispose()
        {
            Function.FunctionRenamed -= OnFunctionRenamed;
            base.Dispose();
        }
    }
}
