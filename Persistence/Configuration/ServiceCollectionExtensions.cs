using Microsoft.Extensions.DependencyInjection;
using System;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Persistence.Repository;

namespace Todo.Backend.Persistence.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddReadRepository(this IServiceCollection collection)
        {
            collection.AddTransient<ITodoReadRepository, TodoReadRepository>();
        }

        public static void AddWriteRepository(this IServiceCollection collection)
        {
            collection.AddTransient<ITodoWriteRepository, TodoWriteRepository>();
        }
    }
}