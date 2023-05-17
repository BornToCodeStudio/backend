using borntocode_backend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace borntocode_backend.Database;

public sealed class ApplicationContext : DbContext
{
    public DbSet<Solution> Solutions { get; set; } = null!;

    public DbSet<SolutionComment> SolutionComments { get; set; } = null!;

    public DbSet<SolutionLike> SolutionLikes { get; set; } = null!;

    public DbSet<CodeTask> Tasks { get; set; } = null!;

    public DbSet<TaskComment> TaskComments { get; set; } = null!;

    public DbSet<TaskLike> TaskLikes { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=45.132.18.239;Port=5432;Database=postgres;Username=postgres;Password=root;");
    }
}