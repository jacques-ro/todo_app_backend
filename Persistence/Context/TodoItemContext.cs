using System;
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

            // TODO: clarify why the npgsql migration creates an integer field in the database schema although there is a bool type in postgres
            modelBuilder.Entity<TodoItem>()
                .Property(t => t.IsCompleted)                
                .HasConversion<int>(); 
        }
    }
}