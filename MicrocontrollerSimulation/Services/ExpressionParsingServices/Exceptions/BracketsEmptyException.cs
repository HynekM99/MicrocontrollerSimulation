using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class BracketsEmptyException : BracketException
    {
        public BracketsEmptyException() : base("Expression contains empty brackets.")
        {
        }
    }
}
