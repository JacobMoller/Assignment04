using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Task>()
                .Property(e => e.state)
                .HasConversion(new EnumToStringConverter<State>());
        }
    }
}