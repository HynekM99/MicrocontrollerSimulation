using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class FunctionsSetupViewModel : ViewModelBase
    {
        private readonly NavigationStore<FunctionsSetupViewModel> _navigationStore;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public FunctionsSetupViewModel(NavigationStore<FunctionsSetupViewModel> navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
