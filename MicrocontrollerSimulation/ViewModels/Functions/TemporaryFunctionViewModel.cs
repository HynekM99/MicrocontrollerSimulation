using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class TemporaryFunctionViewModel : ViewModelBase
    {
        private readonly FunctionsCollection _temporaryFunctions;

        public Function Function { get; }

        public ICommand RemoveTemporaryFunctionCommand { get; }

        public TemporaryFunctionViewModel(FunctionsCollection temporaryFunctions, Function function)
        {
            _temporaryFunctions = temporaryFunctions;
            Function = function;

            RemoveTemporaryFunctionCommand = new RelayCommand(e => RemoveTemporaryFunction());
        }

        private void RemoveTemporaryFunction()
        {
            _temporaryFunctions.Remove(Function);

            List<Function> toRemove = new();

            foreach (var tempFunction in _temporaryFunctions)
            {
                if (tempFunction.Expression.ContainsSubexpression(Function.Expression))
                {
                    toRemove.Add(tempFunction);
                }
            }

            toRemove.ForEach(e => _temporaryFunctions.Remove(e));
        }
    }
}
