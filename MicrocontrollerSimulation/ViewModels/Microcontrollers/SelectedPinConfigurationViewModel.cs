using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.PinConfig;
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
        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;
                SelectedPinInputModeConfigViewModel.OriginalPin = value;
                SelectedPinOutputModeConfigViewModel.OriginalPin = value;
                RestoreConfiguration();
                OnPropertyChanged(nameof(OriginalPin));
            }
        }

        public bool IsConfigDifferent
        {
            get {  return SelectedPinInputModeConfigViewModel.IsConfigDifferent ||
                    SelectedPinOutputModeConfigViewModel.IsConfigDifferent ||
                    (OriginalPin is not null && SelectedPinMode != OriginalPin.PinMode);
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
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel,
            SelectedPinOutputModeConfigViewModel selectedPinOutputModeConfigViewModel)
        {
            SelectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
            SelectedPinOutputModeConfigViewModel = selectedPinOutputModeConfigViewModel;

            _currentConfigViewModel = selectedPinInputModeConfigViewModel;

            SaveConfigurationCommand = new SavePinConfigurationCommand(this);
            RestoreConfigurationCommand = new RelayCommand(e => RestoreConfiguration());

            SelectedPinInputModeConfigViewModel.PropertyChanged += InputPropertyChanged;
            SelectedPinOutputModeConfigViewModel.PropertyChanged += OutputPropertyChanged;
        }

        private void InputPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPinInputModeConfigViewModel.IsConfigDifferent))
            {
                OnPropertyChanged(nameof(IsConfigDifferent));
            }
        }

        private void OutputPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPinOutputModeConfigViewModel.IsConfigDifferent))
            {
                OnPropertyChanged(nameof(IsConfigDifferent));
            }
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



        public override void Dispose()
        {
            SelectedPinInputModeConfigViewModel.PropertyChanged -= InputPropertyChanged;
            SelectedPinOutputModeConfigViewModel.PropertyChanged -= OutputPropertyChanged;
            base.Dispose();
        }
    }
}
