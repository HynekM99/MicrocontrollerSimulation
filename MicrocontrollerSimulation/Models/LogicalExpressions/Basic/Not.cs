using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System.Collections.Generic;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Not : LogicalExpression
    {
        private readonly LogicalExpression _expression;

        public override ICollection<Input> Inputs
        {
            get
            {
                HashSet<Input> inputs = new();
                foreach (var input in _expression.Inputs)
                {
                    inputs.Add(input);
                }
                return inputs;
            }
        }
        public override bool Result => !_expression.Result;
        public override string AsString => $"¬{_expression.AsString}";

        public Not(LogicalExpression expression)
        {
            _expression = expression;
        }
    }
}
