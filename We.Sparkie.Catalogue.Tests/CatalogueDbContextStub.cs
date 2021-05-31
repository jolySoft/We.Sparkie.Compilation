using Microsoft.EntityFrameworkCore;
using We.Sparkie.Catalogue.Api.Repository;

namespace We.Sparkie.Catalogue.Tests
{
    public class CatalogueDbContextStub : CatalogueDbContext
    {
        public CatalogueDbContextStub(DbContextOptions options) : base(options)
        {
        }
    }
}