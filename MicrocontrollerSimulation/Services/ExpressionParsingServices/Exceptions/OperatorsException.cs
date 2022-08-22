using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class OperatorsException : Exception
    {
        public OperatorsException() : base("Different operators on the same level are not allowed.")
        {
        }
    }
}
