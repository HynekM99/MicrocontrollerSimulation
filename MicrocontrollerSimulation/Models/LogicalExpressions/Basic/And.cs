using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class And : LogicalExpression
    {
        private static readonly char EXPRESSION_SIGN = '*';

        public override List<LogicalExpression> LogicalExpressions { get; }
        public override HashSet<Input> Inputs { get; } = new HashSet<Input>();

        public override bool Result
        {
            get
            {
                bool result = true;
                LogicalExpressions.ForEach(e => result &= e.Result);
                return result;
            }
        }
        public override string AsString
        {
            get
            {
                string s = "";
                LogicalExpressions.ForEach(e => s += $"{e.AsString} {EXPRESSION_SIGN} ");
                s = s.Trim().Trim(EXPRESSION_SIGN).Trim();
                return $"({s})";
            }
        }

        public And(List<LogicalExpression> expressions)
        {
            if (expressions.Count < 2)
            {
                throw new ArgumentException("The And expression must be composed of at least 2 other expressions.");
            }

            LogicalExpressions = expressions;

            LogicalExpressions.ForEach(e =>
            {
                foreach (var input in e.Inputs)
                {
                    Inputs.Add(input);
                }
            });

            if (ContainsDuplicateInputs())
            {
                throw new ArgumentException($"Expression cannot contain two different inputs with the same name.");
            }
        }

        public And(params LogicalExpression[] expressions) : this(new List<LogicalExpression>(expressions))
        {
        }
    }
}
