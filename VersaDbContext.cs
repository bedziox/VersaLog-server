using Microsoft.EntityFrameworkCore;
using System;
using VersaLog_server.Models;

namespace VersaLog_server;

public class VersaDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseResult> ExerciseResults { get; set;}
    
    public VersaDbContext(DbContextOptions<VersaDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Training>()
            .HasOne(training => training.User)
            .WithMany(user => user.Trainings)
            .HasForeignKey(training => training.UserId)
            .IsRequired();

        modelBuilder.Entity<Training>()
            .HasMany(t => t.ExerciseResults)
            .WithOne(er => er.Training)
            .HasForeignKey(er => er.TrainingId);

        modelBuilder.Entity<Exercise>()
            .HasMany<ExerciseResult>()
            .WithOne(er => er.Exercise)
            .HasForeignKey(er => er.ExerciseId);


    }
}