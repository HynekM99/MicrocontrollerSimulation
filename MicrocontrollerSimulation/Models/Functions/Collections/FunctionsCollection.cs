using MicrocontrollerSimulation.Models.Functions.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.Collections
{
    public class FunctionsCollection : ObservableCollection<Function>
    {
        public event EventHandler<EventArgs>? FunctionChanged;

        public new void Add(Function function)
        {

            if (this.Any(f => f.Name == function.Name))
            {
                throw new ArgumentException($"An expression with the same name ({function}) is already in the list.");
            }

            function.FunctionChanged += OnFunctionRenamed;

            base.Add(function);
        }

        private void OnFunctionRenamed()
        {
            FunctionChanged?.Invoke(this, new());
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }

        public new void Insert(int index, Function function)
        {
            if (this.Any(f => f.Name == function.Name))
            {
                throw new ArgumentException($"An expression with the same name ({function}) is already in the list.");
            }

            function.FunctionChanged -= OnFunctionRenamed;

            base.Insert(index, function);
        }
    }
}
