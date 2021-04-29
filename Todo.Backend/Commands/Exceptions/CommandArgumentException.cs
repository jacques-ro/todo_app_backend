using System;

namespace Todo.Backend.Commands.Exceptions
{
    public class CommandArgumentException : Exception
    {
        public CommandArgumentException(object value, string argumentName) : base($"Argument '{argumentName}' was provided with an invalid value of '{value}'")
        {
        }
    }
}