using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class BracketException : Exception
    {
        public BracketException() : base("Bracket error.")
        {
        }

        public BracketException(string? message) : base(message)
        {
        }
    }
}
