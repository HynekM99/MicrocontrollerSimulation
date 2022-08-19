using MicrocontrollerSimulation.ViewModels.Base;
using System;

namespace MicrocontrollerSimulation.Stores
{
    public class NavigationStore<TViewModel> where TViewModel : ViewModelBase
    {
        public event Action? CurrentViewModelChanged;

        private ViewModelBase? _currentViewModel;

        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
