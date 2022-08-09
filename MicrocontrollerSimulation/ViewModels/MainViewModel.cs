using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MicrocontrollerSetupViewModel MicrocontrollerSetupViewModel { get; }
        public FunctionsSetupViewModel FunctionsSetupViewModel { get; }

        public MainViewModel(MicrocontrollerSetupViewModel microcontrollerSetupViewModel, FunctionsSetupViewModel functionsSetupViewModel)
        {
            MicrocontrollerSetupViewModel = microcontrollerSetupViewModel;
            FunctionsSetupViewModel = functionsSetupViewModel;
        }
    }
}
