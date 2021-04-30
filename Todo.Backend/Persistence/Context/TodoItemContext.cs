using Microsoft.EntityFrameworkCore;
using Todo.Backend.Persistence.Entities;

namespace Todo.Backend.Persistence.Context
{
    public class TodoItemContext : DbContext
    {
        public TodoItemContext(DbContextOptions<TodoItemContext> options) : base(options)
        {            
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("todos");

            modelBuilder.Entity<TodoItem>()
                .HasKey(i => new {i.Id });
        }
    }
}