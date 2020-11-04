using Microsoft.EntityFrameworkCore;

namespace We.Sparkie.Compilation.Api.Repository
{
    public class CompilationDbContext : DbContext
    {
        public CompilationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}