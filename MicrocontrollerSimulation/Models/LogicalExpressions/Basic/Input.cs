using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.ViewModels.Functions.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Input : LogicalExpression
    {
        public override List<LogicalExpression> LogicalExpressions { get; } = new();
        public override HashSet<Input> Inputs { get; } = new();
        public override bool Result { get { return Value; } }
        public override string AsString { get { return Name; } }

        public bool Value { get; set; } = false;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Name));
                }

                if (_name != value)
                {
                    _name = value;
                    OnExpressionChanged();
                }
            }
        }

        public Input() : this("IN")
        {

        }

        public Input(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;

            Inputs.Add(this);
        }
    }
}
