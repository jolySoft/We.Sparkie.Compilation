using System;
using Microsoft.EntityFrameworkCore;

namespace We.Sparkie.Catalogue.Api.Repository
{
    public class CatalogueDbContext : DbContext
    {
        public DbSet<Entities.Compilation> Compilations { get; set;  }

        public CatalogueDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            var connectionString = Environment.GetEnvironmentVariable("Sparkie.Catalogue.ConnectionString");
            if(string.IsNullOrEmpty(connectionString))
                throw new NullReferenceException("Sql server connection string required, please add it to you environment variables");
    
            optionsBuilder
                .UseSqlServer(connectionString, opt =>
                            opt.MigrationsAssembly("We.Sparkie.Catalogue.Api.Migrations"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }
    }