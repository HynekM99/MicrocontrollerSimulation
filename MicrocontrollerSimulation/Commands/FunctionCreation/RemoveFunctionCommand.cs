using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionCreation
{
    public class RemoveFunctionCommand : CommandBase
    {
        private readonly FunctionsCollection _functions;

        public RemoveFunctionCommand(FunctionsCollection functions)
        {
            _functions = functions;
        }



        public override void Execute(object? parameter)
        {
            
        }
    }
}
