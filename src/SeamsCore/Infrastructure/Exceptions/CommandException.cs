using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamsCore.Infrastructure.Exceptions
{
    public class CommandException : Exception
    {
        public CommandException(string message) : base(message) { }
    }
}
