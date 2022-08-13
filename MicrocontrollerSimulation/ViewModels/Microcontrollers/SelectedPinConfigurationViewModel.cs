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

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinConfigurationViewModel : ViewModelBase
    {
        private readonly PinBase? _originalPin;

        public PinBase? OriginalPin => _originalPin;

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

        public SelectedPinConfigurationViewModel(
            PinBase? originalPin,
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel,
            SelectedPinOutputModeConfigViewModel selectedPinOutputModeConfigViewModel)
        {
            _originalPin = originalPin;

            SelectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
            SelectedPinOutputModeConfigViewModel = selectedPinOutputModeConfigViewModel;
            _currentConfigViewModel = selectedPinInputModeConfigViewModel;

            SelectedPinMode = _originalPin is InputPin ? PinMode.Input : PinMode.Output;

            SelectedPinInputModeConfigViewModel.SelectedDeviceName = _originalPin?.InputDevice?.Name;
            if (_originalPin?.InputDevice is ClockDevice clk)
            {
                SelectedPinInputModeConfigViewModel.ClockFrequency = (int)clk.Frequency;
            }

            
        }
    }
}
