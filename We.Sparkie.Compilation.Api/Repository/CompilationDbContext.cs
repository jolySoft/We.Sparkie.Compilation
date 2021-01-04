using System;
using Microsoft.EntityFrameworkCore;

namespace We.Sparkie.Compilation.Api.Repository
{
    public class CompilationDbContext : DbContext
    {
        public DbSet<Entities.Compilation> Compilations { get; set;  }

        public CompilationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            var connectionString = Environment.GetEnvironmentVariable("Sparkie.Compilation.ConnectionString");
            if(string.IsNullOrEmpty(connectionString))
                throw new NullReferenceException("Sql server connection string required, please add it to you environment variables");
    
            optionsBuilder
                .UseSqlServer(connectionString, opt =>
                            opt.MigrationsAssembly("We.Sparkie.Compilation.Api.Migrations"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }
    }