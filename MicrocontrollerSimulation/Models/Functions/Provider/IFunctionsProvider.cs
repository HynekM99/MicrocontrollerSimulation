using MicrocontrollerSimulation.Models.Functions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.Provider
{
    public interface IFunctionsProvider
    {
        event Action? AvailableFunctionsChanged;

        bool CanProvide(string? functionName);
        Function? Request(string? functionName);
    }
}
