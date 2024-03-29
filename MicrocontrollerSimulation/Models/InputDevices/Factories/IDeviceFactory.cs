﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices.Factories
{
    public interface IDeviceFactory
    {
        ButtonDevice CreateButtonDevice();
        SwitchDevice CreateSwitchDevice();
        ClockDevice CreateClockDevice();
        List<InputDevice> CreateAllDevices();

        InputDevice? CreateDevice<T>() where T : InputDevice?
        {
            return typeof(T).Name switch
            {
                nameof(ButtonDevice) => CreateButtonDevice(),
                nameof(SwitchDevice) => CreateSwitchDevice(),
                nameof(ClockDevice) => CreateClockDevice(),
                _ => null
            };
        }
    }
}
