using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Custom
{
    public class CustomExpression : LogicalExpression
    {
        private readonly LogicalExpression _logicalExpression;

        public override List<LogicalExpression> LogicalExpressions { get; } = new();
        public override HashSet<Input> Inputs => _logicalExpression.Inputs;
        public override bool Result => _logicalExpression.Result;
        public override string AsString => _logicalExpression.AsString;

        

        public CustomExpression(LogicalExpression expression)
        {
            _logicalExpression = expression;
            LogicalExpressions.Add(expression);
        }
    }
}
