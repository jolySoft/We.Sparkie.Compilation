using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using We.Sparkie.Compilation.Api.Controllers;
using We.Sparkie.Compilation.Api.Repository;
using We.Sparkie.Compilation.Api.Entities;
using Xunit;

namespace We.Sparkie.Compilation.Tests
{
    public class CompilationControllerTest
    {
        private Repository<Api.Entities.Compilation> _repository;
        private CompilationDbContext _dbContext;
        private List<Api.Entities.Compilation> _entities;
        private CompilationController _controller;

        public CompilationControllerTest()
        {
            var builder = new DbContextOptionsBuilder<CompilationDbContext>();
            builder.UseInMemoryDatabase("CompilationDb");
            var options = builder.Options;

            _dbContext = new CompilationDbContext(options);


            _repository = new Repository<Api.Entities.Compilation>(_dbContext);
            _controller = new CompilationController(_repository);
        }


        [Fact]
        public async Task CanGetAll()
        {
            BuildCollection();
            var result = await _controller.Get();

            result.Should().BeEquivalentTo(_entities);
        }

        [Fact]
        public async Task CanGetById()
        {
            BuildCollection();
            var entity = _entities[0];

            var result = ((OkObjectResult)await _controller.Get(entity.Id)).Value;

            result.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task CanPostAnEntity()
        {
            var entity = new Api.Entities.Compilation
            {
                CreatedBy = "Jolyon Wharton",
                CompilationType = CompilationType.PlayList
            };

            await _controller.Post(entity);

            _dbContext.Set<Api.Entities.Compilation>().Single(e => e.CreatedBy == "Jolyon Wharton").Should().NotBeNull();
        }

        [Fact]
        public void CanPutAnEntity()
        {
            BuildCollection();
            var entity = _entities[1];
            entity.CreatedBy = "Brian Blessed";
            entity.CompilationType = CompilationType.PlayList;

            _controller.Put(entity.Id, entity);

            _dbContext.Set<Api.Entities.Compilation>().Single(e => e.Id == entity.Id).Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task CanPatchAnEntity()
        {
            BuildCollection();
            var operations = new List<Operation<Api.Entities.Compilation>>
            {
                new Operation<Api.Entities.Compilation>("replace", "/CreatedBy", null, "Palpatine")
            };
            var patch = new JsonPatchDocument<Api.Entities.Compilation>(operations, new DefaultContractResolver());
            var id = _entities[2].Id;

            await _controller.Patch(id, patch);

            _dbContext.Set<Api.Entities.Compilation>().Single(e => e.Id == id).CreatedBy.Should().Be("Palpatine");
        }

        [Fact]
        public async Task CanDeleteAnEntity()
        {
            BuildCollection();
            var id = _entities[0].Id;

            await _controller.Delete(id);

            _dbContext.Set<Api.Entities.Compilation>().Count().Should().Be(2);
        }

        private void BuildCollection()
        {
            _entities = new List<Api.Entities.Compilation>
            {
                new Api.Entities.Compilation
                {
                    CompilationType = CompilationType.Album,
                    CreatedBy = "Afsul"
                },
                new Api.Entities.Compilation
                {
                    CreatedBy = "Kieran Iles",
                    CompilationType = CompilationType.Ep
                },
                new Api.Entities.Compilation
                {
                    CreatedBy = "Peter Srank",
                    CompilationType = CompilationType.Soundtrack
                }
            };

            _dbContext.Compilations.RemoveRange(_dbContext.Compilations);
            _dbContext.SaveChanges();
            _dbContext.Compilations.AddRange(_entities);
            _dbContext.SaveChanges();
        }
    }
}