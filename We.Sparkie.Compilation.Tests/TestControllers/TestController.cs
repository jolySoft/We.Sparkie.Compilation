using We.Sparkie.Compilation.Api.Controllers;
using We.Sparkie.Compilation.Api.Repository;
using We.Sparkie.Compilation.Tests.TestEntities;

namespace We.Sparkie.Compilation.Tests.TestControllers
{
    public class TestController : EntityController<TestEntity>
    {
        public TestController(Repository<TestEntity> repository) : base(repository)
        {
        }
    }
}