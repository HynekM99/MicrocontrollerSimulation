using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System.Collections.Generic;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Not : LogicalExpression
    {
        private readonly LogicalExpression _logicalExpression;

        public override List<LogicalExpression> LogicalExpressions { get; } = new();
        public override HashSet<Input> Inputs => _logicalExpression.Inputs;
        public override bool Result => !_logicalExpression.Result;
        public override string AsString => $"¬{_logicalExpression.AsString}";

        public Not(LogicalExpression expression)
        {
            _logicalExpression = expression;
            LogicalExpressions.Add(expression);
        }
    }
}
