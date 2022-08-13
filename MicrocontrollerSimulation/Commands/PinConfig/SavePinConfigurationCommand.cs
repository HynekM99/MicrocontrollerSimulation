using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.PinConfig
{
    public class SavePinConfigurationCommand : CommandBase
    {
        private readonly SelectedPinConfigurationViewModel _selectedPinConfigurationViewModel;

        public SavePinConfigurationCommand(SelectedPinConfigurationViewModel selectedPinConfigurationViewModel)
        {
            _selectedPinConfigurationViewModel = selectedPinConfigurationViewModel;

            _selectedPinConfigurationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_selectedPinConfigurationViewModel.IsConfigDifferent))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _selectedPinConfigurationViewModel.IsConfigDifferent &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _selectedPinConfigurationViewModel.SaveConfiguration();
        }
    }
}
