using MicrocontrollerSimulation.Models.Functions.Provider;
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
        private readonly IDeviceFactory _deviceFactory;
        private readonly IFunctionsProvider _functionsProvider;

        public PinBase? OriginalPin => _originalPin;

        private PinMode _selectedPinMode;
        public PinMode SelectedPinMode
        {
            get { return _selectedPinMode; }
            set
            {
                _selectedPinMode = value;
                OnPropertyChanged(nameof(SelectedPinMode));
            }
        }

        public SelectedPinInputModeConfigViewModel SelectedPinInputModeConfigViewModel { get; }

        public SelectedPinConfigurationViewModel(
            PinBase? originalPin,
            IDeviceFactory deviceFactory,
            IFunctionsProvider functionsProvider,
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel)
        {
            _originalPin = originalPin;
            _deviceFactory = deviceFactory;
            _functionsProvider = functionsProvider;

            SelectedPinMode = _originalPin is InputPin ? PinMode.Input : PinMode.Output;
            SelectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
        }
    }
}
