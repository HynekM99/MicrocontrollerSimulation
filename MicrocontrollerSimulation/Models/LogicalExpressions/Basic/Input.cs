using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Input : LogicalExpression
    {
        public bool Value { get; set; } = false;

        public override List<LogicalExpression> LogicalExpressions { get; } = new();
        public override HashSet<Input> Inputs { get; } = new();

        public override bool Result { get { return Value; } }
        public override string AsString { get; }

        public Input() : this("IN")
        {

        }

        public Input(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            AsString = name; 

            Inputs.Add(this);
        }
    }
}
