using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Commands.Exceptions;
using Todo.Backend.Contract.Repository;

namespace Todo.Backend.Commands
{
    internal abstract class TodoItemCommandHandlerBase<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
    {
        protected readonly ITodoWriteRepository Repository;

        public TodoItemCommandHandlerBase(ITodoWriteRepository repository)
        {
            Repository = repository;
        }

        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);

        #region Common Assertions
        protected static void AssertValidTitle(string title)
        {
            if(title == string.Empty)
            {
                throw new CommandArgumentException(title, nameof(title));
            }
        }

        protected static void AssertValidGuid(Guid id)
        {
            if(id == Guid.Empty)            
            {
                throw new CommandArgumentException(id, nameof(id));
            }
        }
        #endregion
    }    
}