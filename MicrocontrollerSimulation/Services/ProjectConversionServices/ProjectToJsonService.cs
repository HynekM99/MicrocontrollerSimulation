using MicrocontrollerSimulation.JDOs;
using MicrocontrollerSimulation.Models.Functions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MicrocontrollerSimulation.JDOs.Functions;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.JDOs.Expressions;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.JDOs.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.JDOs.InputDevices;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;

namespace MicrocontrollerSimulation.Services.ProjectConversionServices
{
    public class ProjectToJsonService : IProjectToJsonService
    {
        public string Convert(CurrentProject project)
        {


            ProjectJDO projectJDO = new()
            {
                Name = project.ProjectInfo.Name,
                Functions = ConvertFunctions(project.ProjectInfo.Functions),
                Microcontroller = ConvertMicrocontroller(project.ProjectInfo.Microcontroller)
            };

            var settings = new JsonSerializerSettings() 
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(projectJDO, settings);
        }

        private List<FunctionJDO> ConvertFunctions(FunctionsCollection functions)
        {
            List<FunctionJDO> functionJDOs = new();

            foreach (var function in functions)
            {
                functionJDOs.Add(ConvertFunction(function));
            }

            return functionJDOs;
        }

        private FunctionJDO ConvertFunction(Function function)
        {
            FunctionJDO functionJDO = new()
            {
                Name = function.Name,
                LogicalExpression = ConvertExpression(function.Expression)
            };

            foreach (var input in function.Expression.Inputs)
            {
                functionJDO.LogicalExpression.Inputs.Add(new InputJDO() { Name = input.AsString });
            }

            return functionJDO;
        }

        private LogicalExpressionJDO ConvertExpression(LogicalExpression expression)
        {
            LogicalExpressionJDO expressionJDO;

            if (expression is CustomExpression)
            {
                expressionJDO = new CustomExpressionJDO();
                expressionJDO.LogicalExpressions.Add(ConvertExpression(expression.LogicalExpressions.First()));
            }
            else if (expression is Input input)
            {
                expressionJDO = new InputJDO
                {
                    Name = input.AsString
                };
            }
            else if (expression is Not)
            {
                expressionJDO = new NotJDO();
                expressionJDO.LogicalExpressions.Add(ConvertExpression(expression.LogicalExpressions.First()));
            }
            else if (expression is And)
            {
                expressionJDO = new AndJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }
            else if (expression is Or)
            {
                expressionJDO = new OrJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }
            else
            {
                expressionJDO = new XorJDO();
                expression.LogicalExpressions.
                    ForEach(e => expressionJDO.LogicalExpressions.Add(ConvertExpression(e)));
            }

            return expressionJDO;
        }

        private MicrocontrollerJDO ConvertMicrocontroller(Microcontroller mcu)
        {
            MicrocontrollerJDO mcuJDO = new();

            foreach (var pin in mcu.Pins)
            {
                mcuJDO.Pins.Add(ConvertPin(pin));
            }

            return mcuJDO;
        }

        private DigitalPinJDO ConvertPin(DigitalPin pin)
        {
            var pinJDO = new DigitalPinJDO();
            pinJDO.Number = pin.Number;
            pinJDO.OutputMode = pin.PinMode == PinMode.Output;
            pinJDO.InputDevice = ConvertInputDevice(pin.InputDevice);
            pinJDO.FunctionConfig = ConvertFunctionConfig(pin.FunctionConfig);

            return pinJDO;
        }

        private InputDeviceJDO? ConvertInputDevice(InputDevice? device)
        {
            if (device is ButtonDevice)
            {
                return new ButtonDeviceJDO();
            }
            else if (device is SwitchDevice)
            {
                return new SwitchDeviceJDO();
            }
            else if (device is ClockDevice clk)
            {
                return new ClockDeviceJDO() { Interval = (int)clk.Interval };
            }
            return null;
        }

        private FunctionConfigJDO? ConvertFunctionConfig(FunctionConfig? functionConfig)
        {
            if (functionConfig is null) return null;

            FunctionConfigJDO configJDO = new();

            configJDO.Function = functionConfig.Function!.Name;

            foreach (var entry in functionConfig.ConfigEntries!)
            {
                var entryJDO = new ConfigEntryJDO
                {
                    Input = entry.Input.AsString,
                    PinNumber = entry.PinNumber
                };

                configJDO.ConfigEntries.Add(entryJDO);
            }

            return configJDO;
        }
    }
}
