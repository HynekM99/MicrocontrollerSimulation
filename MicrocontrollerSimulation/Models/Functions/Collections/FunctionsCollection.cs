using MicrocontrollerSimulation.Models.Functions.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.Collections
{
    public class FunctionsCollection : ObservableCollection<Function>
    {
        public new void Add(Function function)
        {
            if (this.Any(f => f.Name == function.Name))
            {
                throw new ArgumentException($"An expression with the same name ({function}) is already in the list.");
            }

            base.Add(function);
        }

        public new void Insert(int index, Function function)
        {
            if (this.Any(f => f.Name == function.Name))
            {
                throw new ArgumentException($"An expression with the same name ({function}) is already in the list.");
            }

            base.Insert(index, function);
        }
    }
}
