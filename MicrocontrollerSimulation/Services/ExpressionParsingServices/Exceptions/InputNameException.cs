using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class InputNameException : Exception
    {
        public InputNameException() : base("Input names can only contain letters, digits and underscores. cannot begin with a digit.")
        {
        }

        public InputNameException(string? message) : base(message)
        {
        }
    }
}
