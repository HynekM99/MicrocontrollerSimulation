﻿using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Base
{
    public abstract class LogicalExpression
    {
        public abstract List<LogicalExpression> LogicalExpressions { get; }
        public abstract HashSet<Input> Inputs { get; }
        public abstract bool Result { get; }
        public abstract string AsString { get; }

        protected bool ContainsDuplicateInputs()
        {
            return Inputs.Distinct().Count() != Inputs.Count;
        }

        public override bool Equals(object? obj)
        {
            return obj is LogicalExpression expression &&
                   ToString() == expression.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ToString());
        }

        public override string ToString()
        {
            return AsString;
        }
    }
}