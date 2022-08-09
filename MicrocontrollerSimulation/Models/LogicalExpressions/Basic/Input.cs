using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Input : LogicalExpression
    {
        public bool Value { get; set; } = false;

        public override ICollection<Input> Inputs
        {
            get
            {
                HashSet<Input> inputs = new();
                inputs.Add(this);
                return inputs;
            }
        }
        public override bool Result => Value;
        public override string AsString { get; }

        public Input() : this("IN")
        {

        }

        public Input(string name)
        {
            AsString = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
