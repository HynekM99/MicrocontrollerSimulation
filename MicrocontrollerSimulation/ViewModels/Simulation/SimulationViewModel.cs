using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Simulation
{
    public class SimulationViewModel : ViewModelBase
    {
        private readonly Microcontroller _microcontroller;

        public PinContentViewModel?[] PinContentViewModels { get; }

        public SimulationViewModel(Microcontroller microcontroller)
        {
            _microcontroller = microcontroller;

            var pins = _microcontroller.Pins;
            PinContentViewModels = new PinContentViewModel[pins.Length];

            for (int i = 0; i < pins.Length; i++)
            {
                PinContentViewModels[i] = GetPinViewModel(pins[i]);
            }

            _microcontroller.IsRunning = true;
        }

        ~SimulationViewModel()
        {

        }

        private PinContentViewModel? GetPinViewModel(DigitalPin pin)
        {
            
            if (pin.InputDevice is ButtonDevice btn)
            {
                return new ButtonDeviceViewModel(btn);
            }
            else if (pin.InputDevice is SwitchDevice sw)
            {
                return new SwitchDeviceViewModel(sw);
            }
            else if (pin.InputDevice is ClockDevice clk)
            {
                return new ClockDeviceViewModel(clk);
            }
            else if (pin.PinMode == PinMode.Output)
            {
                return new OutputSignalViewModel(pin);
            }
            return null;
        }
    }
}
