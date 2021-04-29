using System;
using System.Runtime.Serialization;
using Todo.Backend.Models;

namespace Todo.Backend.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(Guid id) : base($"A todo item with {id} could not be found.")
        {}
    }
}