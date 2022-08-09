using MicrocontrollerSimulation.Stores;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationStore<MainWindowViewModel> _navigationStore;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore<MainWindowViewModel> navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
