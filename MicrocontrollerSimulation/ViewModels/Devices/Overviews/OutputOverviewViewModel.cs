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
        }
    }
}
