using Amazon.Internal;
using Microsoft.EntityFrameworkCore;
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
    
}do