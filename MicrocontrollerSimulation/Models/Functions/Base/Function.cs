using MicrocontrollerSimulation.Models.Functions.FunctionEventArgs;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;

namespace MicrocontrollerSimulation.Models.Functions.Base
{
    public class Function
    {
        public event Action? FunctionChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(value);
                }

                if (_name != value)
                {
                    _name = value;
                    OnFunctionChanged();
                }
            }
        }

        public LogicalExpression Expression { get; }

        public Function(string name, LogicalExpression expression)
        {
            _name = name;
            Expression = expression;

            foreach (var input in expression.Inputs)
            {
                input.ExpressionChanged += OnFunctionChanged;
            }
        }

        public void OnFunctionChanged()
        {
            FunctionChanged?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
