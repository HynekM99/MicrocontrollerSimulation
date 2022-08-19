using MicrocontrollerSimulation.Commands.Base;
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
        private readonly SelectedPinInputModeConfigViewModel _selectedPinInputModeConfigViewModel;
        private readonly SelectedPinOutputModeConfigViewModel _selectedPinOutputModeConfigViewModel;

        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;

                _selectedPinInputModeConfigViewModel.OriginalPin = value;
                _selectedPinOutputModeConfigViewModel.OriginalPin = value;

                PinMode = OriginalPin is null ? PinMode.Input : OriginalPin.PinMode;

                OnPropertyChanged(nameof(OriginalPin));
            }
        }

        public PinMode PinMode
        {
            get { return _originalPin is null ? PinMode.Input : _originalPin.PinMode; }
            set
            {
                if (_originalPin is not null)
                {
                    _originalPin.PinMode = value;
                }

                CurrentConfigViewModel = (value == PinMode.Input) ?
                    _selectedPinInputModeConfigViewModel :
                    _selectedPinOutputModeConfigViewModel;
            }
        }

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
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel,
            SelectedPinOutputModeConfigViewModel selectedPinOutputModeConfigViewModel)
        {
            _selectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
            _selectedPinOutputModeConfigViewModel = selectedPinOutputModeConfigViewModel;

            _currentConfigViewModel = selectedPinInputModeConfigViewModel;
        }
    }
}
