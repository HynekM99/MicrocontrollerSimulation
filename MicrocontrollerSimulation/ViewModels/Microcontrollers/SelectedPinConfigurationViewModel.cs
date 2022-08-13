using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.PinConfig;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinConfigurationViewModel : ViewModelBase
    {
        public DigitalPin? OriginalPin { get; }

        public bool IsConfigDifferent
        {
            get {
                return SelectedPinInputModeConfigViewModel.IsConfigDifferent ||
                    SelectedPinOutputModeConfigViewModel.IsConfigDifferent ||
                    SelectedPinMode != OriginalPin!.PinMode;
            }
        }

        private PinMode _selectedPinMode;
        public PinMode SelectedPinMode
        {
            get { return _selectedPinMode; }
            set
            {
                _selectedPinMode = value;
                OnPropertyChanged(nameof(SelectedPinMode));
                CurrentConfigViewModel = value == PinMode.Input ?
                    SelectedPinInputModeConfigViewModel :
                    SelectedPinOutputModeConfigViewModel;
                OnPropertyChanged(nameof(IsConfigDifferent));
            }
        }

        public SelectedPinInputModeConfigViewModel SelectedPinInputModeConfigViewModel { get; }
        public SelectedPinOutputModeConfigViewModel SelectedPinOutputModeConfigViewModel { get; }

        private ViewModelBase _currentConfigViewModel;
        public ViewModelBase CurrentConfigViewModel
        {
            get { return _currentConfigViewModel; }
            set
            {
                _currentConfigViewModel = value;
                OnPropertyChanged(nameof(CurrentConfigViewModel));
            }
        }

        public ICommand SaveConfigurationCommand { get; }
        public ICommand RestoreConfigurationCommand { get; }

        public SelectedPinConfigurationViewModel(
            DigitalPin? originalPin,
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel,
            SelectedPinOutputModeConfigViewModel selectedPinOutputModeConfigViewModel)
        {
            OriginalPin = originalPin;

            SelectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
            SelectedPinOutputModeConfigViewModel = selectedPinOutputModeConfigViewModel;
            _currentConfigViewModel = selectedPinInputModeConfigViewModel;

            if (OriginalPin is null) return;

            SelectedPinMode = OriginalPin is null ? PinMode.Input : OriginalPin.PinMode;

            SaveConfigurationCommand = new SavePinConfigurationCommand(this);
            RestoreConfigurationCommand = new RelayCommand(e => RestoreConfiguration());

            SelectedPinInputModeConfigViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedPinInputModeConfigViewModel.IsConfigDifferent))
                {
                    OnPropertyChanged(nameof(IsConfigDifferent));
                }
            };
            SelectedPinOutputModeConfigViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedPinOutputModeConfigViewModel.IsConfigDifferent))
                {
                    OnPropertyChanged(nameof(IsConfigDifferent));
                }
            };
        }

        public void RestoreConfiguration()
        {
            SelectedPinMode = OriginalPin is null ? PinMode.Input : OriginalPin.PinMode;
            SelectedPinInputModeConfigViewModel.RestoreConfiguration();
            SelectedPinOutputModeConfigViewModel.RestoreConfiguration();
        }

        public void SaveConfiguration()
        {
            OriginalPin!.PinMode = SelectedPinMode;
            SelectedPinInputModeConfigViewModel.SaveConfiguration();
            SelectedPinOutputModeConfigViewModel.SaveConfiguration();
        }
    }
}
