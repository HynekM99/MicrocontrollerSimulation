using MicrocontrollerSimulation.Models.Microcontroller;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class MicrocontrollerSetupViewModel : ViewModelBase
    {
        private readonly Microcontroller _microcontroller;
        private readonly NavigationStore<MicrocontrollerSetupViewModel> _navigationStore;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public MicrocontrollerSetupViewModel(
            Microcontroller microcontroller, 
            NavigationStore<MicrocontrollerSetupViewModel> navigationStore)
        {
            _microcontroller = microcontroller;
            _navigationStore = navigationStore;
        }
    }
}
