using Microsoft.EntityFrameworkCore;

namespace We.Sparkie.Compilation.Api.Repository
{
    public class CompilationDbContext : DbContext
    {
        public DbSet<Entities.Compilation> Compilations { get; set;  }

        public CompilationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}