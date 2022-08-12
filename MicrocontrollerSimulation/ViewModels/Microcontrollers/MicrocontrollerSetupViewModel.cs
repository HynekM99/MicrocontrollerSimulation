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
        private readonly NavigationStore<MicrocontrollerSetupViewModel> _navigationStore;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public MicrocontrollerSetupViewModel(NavigationStore<MicrocontrollerSetupViewModel> navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
