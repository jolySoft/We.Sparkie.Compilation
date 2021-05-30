using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using We.Sparkie.Compilation.Api;
using We.Sparkie.Compilation.Api.Entities;
using We.Sparkie.Compilation.Api.Repository;
using Xunit;

namespace We.Sparkie.Compilation.Tests
{
    public class CompilationTests
    {
        private TestServer _testServer;
        public CatalogueDbContextStub _dbStub;
        private HttpClient _client; 

        public CompilationTests()
        {
            var dbBuilder = new DbContextOptionsBuilder<CatalogueDbContext>();
            dbBuilder.UseInMemoryDatabase("CatalogueDb");
            var option = dbBuilder.Options;
            _dbStub = new CatalogueDbContextStub(option);
            Startup.OnServiceCollection = services =>
            {
                services.AddSingleton<CatalogueDbContext>((_) => _dbStub);
            };

            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _testServer.CreateClient();
        }

        [Fact]
        public async Task Can_create_a_compilation()
        {
            var compilation = new Api.Entities.Compilation
            {
                CompilationType = CompilationType.PlayList,
                CreatedBy = "JolySoft",
                Tracks = new List<Track>
                {
                    new()
                    {
                        Name = "Upside Down Cops",
                        Artist = new Artist
                        {
                            Name = "Lorn"
                        },
                        DigitalAssetId = Guid.NewGuid(),
                    },
                    new()
                    {
                        Name = "Uranium",
                        Artist = new Artist
                        {
                            Name = "Radioactive Man"
                        }
                    }
                }
            };
            var stringContent = JsonConvert.SerializeObject(compilation);

            var result = await _client.PostAsync("api/compilation", new StringContent(stringContent, Encoding.UTF8, "application/json"));
            var jsonString = await result.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<Guid>(jsonString);

            var saved = await _dbStub.Compilations.SingleAsync(c => c.Id == id);
            saved.Should().BeEquivalentTo(compilation,
                opt =>
                    opt.Excluding((IMemberInfo mi) => mi.SelectedMemberPath.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase)));
        }
    }

    public class CatalogueDbContextStub : CatalogueDbContext
    {
        public CatalogueDbContextStub(DbContextOptions options) : base(options)
        {
        }
    }
}