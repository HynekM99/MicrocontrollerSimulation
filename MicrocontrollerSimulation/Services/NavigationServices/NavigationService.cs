using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using System;

namespace MicrocontrollerSimulation.Services.NavigationServices
{
    public class NavigationService<TOwnerViewModel, TViewModel> : INavigationService 
        where TViewModel : ViewModelBase 
        where TOwnerViewModel : ViewModelBase
    {
        private readonly NavigationStore<TOwnerViewModel> _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore<TOwnerViewModel> navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
