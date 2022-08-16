using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrocontrollerSimulation.JDOs;
using MicrocontrollerSimulation.JDOs.Expressions;
using MicrocontrollerSimulation.JDOs.Functions;
using MicrocontrollerSimulation.JDOs.InputDevices;
using MicrocontrollerSimulation.JDOs.Microcontrollers;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using MicrocontrollerSimulation.Models.Project;
using Newtonsoft.Json;

namespace MicrocontrollerSimulation.Services.ProjectConversionServices
{
    public class JsonToProjectService : IJsonToProjectService
    {
        public ProjectInfo Unconvert(string json)
        {
            var settings = new JsonSerializerSettings() 
            { 
                TypeNameHandling = TypeNameHandling.Auto 
            };
            var projectJDO = JsonConvert.DeserializeObject<ProjectJDO>(json, settings);

            if (projectJDO is null) return ProjectInfo.GetDefaultProject();

            var projectName = projectJDO.Name;

            FunctionsCollection functions = UnconvertFunctions(projectJDO.Functions);
            Microcontroller microcontroller = UnconvertMicrocontroller(projectJDO.Microcontroller, functions);

            return new ProjectInfo(projectName, functions, microcontroller);
        }

        private FunctionsCollection UnconvertFunctions(List<FunctionJDO> functionJDOs)
        {
            FunctionsCollection functions = new();

            foreach (var functionJDO in functionJDOs)
            {
                functions.Add(UnconvertFunction(functionJDO));
            }

            return functions;
        }

        private Function UnconvertFunction(FunctionJDO functionJDO)
        {
            string name = functionJDO.Name;
            var expression = UnconvertExpression(functionJDO.LogicalExpression);
            return new Function(name, expression);
        }

        private LogicalExpression UnconvertExpression(LogicalExpressionJDO expressionJDO)
        {
            LogicalExpression expression;

            if (expressionJDO is CustomExpressionJDO)
            {
                expression = new CustomExpression(UnconvertExpression(expressionJDO.LogicalExpressions.First()));
            }
            else if (expressionJDO is InputJDO inputJDO)
            {
                expression = new Input(inputJDO.Name);
            }
            else if (expressionJDO is NotJDO)
            {
                expression = new Not(UnconvertExpression(expressionJDO.LogicalExpressions.First()));
            }
            else if (expressionJDO is AndJDO)
            {
                var list = new List<LogicalExpression>();
                expressionJDO.LogicalExpressions.
                    ForEach(e => list.Add(UnconvertExpression(e)));
                expression = new And(list);
            }
            else if (expressionJDO is OrJDO)
            {
                var list = new List<LogicalExpression>();
                expressionJDO.LogicalExpressions.
                    ForEach(e => list.Add(UnconvertExpression(e)));
                expression = new Or(list);
            }
            else
            {
                var list = new List<LogicalExpression>();
                expressionJDO.LogicalExpressions.
                    ForEach(e => list.Add(UnconvertExpression(e)));
                expression = new Xor(list);
            }

            return expression;
        }

        private Microcontroller UnconvertMicrocontroller(MicrocontrollerJDO mcuJDO, FunctionsCollection functions)
        {
            Microcontroller mcu = new();

            foreach (var pinJDO in mcuJDO.Pins)
            {
                var pin = mcu.Pins.Where(p => p.Number == pinJDO.Number).First();
                pin.PinMode = pinJDO.OutputMode ? PinMode.Output : PinMode.Input;
                pin.InputDevice = UnconvertInputDevice(pinJDO.InputDevice);
                pin.FunctionConfig = UnconvertFunctionConfig(pinJDO.FunctionConfig, functions);
            }

            return mcu;
        }

        private InputDevice? UnconvertInputDevice(InputDeviceJDO? inputDeviceJDO)
        {
            if (inputDeviceJDO is ButtonDeviceJDO)
            {
                return new ButtonDevice();
            }
            else if (inputDeviceJDO is SwitchDeviceJDO)
            {
                return new SwitchDevice();
            }
            else if (inputDeviceJDO is ClockDeviceJDO clk)
            {
                return new ClockDevice
                {
                    Frequency = clk.Frequency
                };
            }

            return null;
        }

        private FunctionConfig? UnconvertFunctionConfig(FunctionConfigJDO? configJDO, FunctionsCollection _functions)
        {
            if (configJDO is null) return null;

            FunctionConfig? functionConfig = new(configJDO.Function, _functions);

            foreach (var entry in configJDO.ConfigEntries)
            {
                functionConfig.ConfigEntries!
                    .Where(e => e.Input.AsString == entry.Input)
                    .First()
                    .PinNumber = entry.PinNumber;
            }

            return functionConfig;
        }
    }
}
