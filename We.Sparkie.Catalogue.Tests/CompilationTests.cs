using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using We.Sparkie.Catalogue.Api;
using We.Sparkie.Catalogue.Api.Entities;
using We.Sparkie.Catalogue.Api.Repository;
using Xunit;

namespace We.Sparkie.Catalogue.Tests
{
    public class CompilationTests
    {
        private TestServer _testServer;
        public CatalogueDbContextStub _dbStub;
        private HttpClient _client;
        private Compilation _compilation;
        private string _url;

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

            _url = "api/compilation";

            BuildCompilation();
            _dbStub.Compilations.Add(_compilation);
            _dbStub.SaveChanges();
        }

        [Fact]
        public async Task Can_create_a_compilation()
        {
            BuildCompilation();
            var stringContent = JsonConvert.SerializeObject(_compilation);

            var result = await _client.PostAsync(_url, new StringContent(stringContent, Encoding.UTF8, "application/json"));
            var jsonString = await result.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<Guid>(jsonString);

            var saved = await _dbStub.Compilations.SingleAsync(c => c.Id == id);
            saved.Should().BeEquivalentTo(_compilation,
                opt =>
                    opt.Excluding((IMemberInfo mi) => mi.SelectedMemberPath.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Fact]
        public async Task Can_update_a_compilation()
        {
            var id = _compilation.Id;
            _dbStub.Entry<Compilation>(_compilation).State = EntityState.Detached;

            var update = new Compilation
            {
                CompilationType = CompilationType.PlayList,
                CreatedBy = "updated",
                Tracks = new List<Track>
                {
                    new()
                    {
                        Name = "updated",
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
                            Name = "updated"
                        }
                    }
                }
            };

            var stringContent = JsonConvert.SerializeObject(update);

            var result = await _client.PutAsync($"{_url}/{id}", new StringContent(stringContent, Encoding.UTF8, "application/json"));

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var persisted = _dbStub.Compilations.Single(c => c.Id == id);
            persisted.CreatedBy.Should().Be("updated");
            persisted.Tracks[0].Name.Should().Be("updated");
            persisted.Tracks[1].Artist.Name.Should().Be("updated");
        }

        [Fact]
        public async Task Can_get_by_id()
        {
            AddCompilationToDb();

            var response = await _client.GetAsync($"{_url}/{_compilation.Id}");
            var stringContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Compilation>(stringContent);

            result.CreatedBy.Should().Be("jolySoft");
            result.CompilationType.Should().Be(CompilationType.PlayList);
        }

        [Fact]
        public async Task Can_get_all()
        {
            AddCompilationToDb();
        
            var response = await _client.GetAsync(_url);
            var stringContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Compilation>>(stringContent);

            result.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task Can_patch_a_compilation()
        {
            AddCompilationToDb();

            var patch = new JsonPatchDocument<Compilation>().Replace(c => c.CreatedBy, "Palpatine");
            var stringContent = JsonConvert.SerializeObject(patch);

            var result = await _client.PatchAsync($"{_url}/{_compilation.Id}", new StringContent(stringContent, Encoding.UTF8, "application/json-patch+json"));

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var persisted = _dbStub.Compilations.Single(c => c.Id == _compilation.Id);
            persisted.CreatedBy.Should().Be("Palpatine");
        }

        [Fact]
        public async Task Can_delete_a_compilation()
        {
            AddCompilationToDb();

            var response = await _client.DeleteAsync($"{_url}/{_compilation.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deleted = _dbStub.Compilations.SingleOrDefault(c => c.Id == _compilation.Id);
            deleted.Should().BeNull();

        }

        private void AddCompilationToDb()
        {
            BuildCompilation();
            _dbStub.Compilations.Add(_compilation);
            _dbStub.SaveChanges();
        }

        private void BuildCompilation()
        {
            _compilation = new Compilation
            {
                CompilationType = CompilationType.PlayList,
                CreatedBy = "jolySoft",
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
        }
    }
}