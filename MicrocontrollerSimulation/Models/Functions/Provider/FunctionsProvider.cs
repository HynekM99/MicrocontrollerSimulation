using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.Provider
{
    public class FunctionsProvider : IFunctionsProvider
    {
        public event Action? AvailableFunctionsChanged;

        private readonly FunctionsCollection _functions;

        public FunctionsProvider(FunctionsCollection functions)
        {
            _functions = functions;

            _functions.CollectionChanged += (s, e) => AvailableFunctionsChanged?.Invoke();
        }

        public bool CanProvide(string? functionName)
        {
            return functionName is not null && _functions.Any(f => f.Name == functionName);
        }

        public Function? Request(string? functionName)
        {
            return _functions.Where(f => f.Name == functionName).FirstOrDefault();
        }

        public List<string> GetAvailableFunctions()
        {
            return _functions.ToList().ConvertAll(f => f.Name);
        }
    }
}
