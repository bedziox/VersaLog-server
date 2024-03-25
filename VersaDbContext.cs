using Microsoft.EntityFrameworkCore;
using System;
using VersaLog_server.Models;

namespace VersaLog_server;

public class VersaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    
    public VersaDbContext(DbContextOptions<VersaDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Training>()
            .HasOne<User>()
            .WithMany(t => t.Trainings)
            .HasForeignKey(x => x.TrainingId);
        modelBuilder.Entity<Exercise>()
            .HasMany<Training>()
            .WithMany(t => t.Exercises);
    }
}