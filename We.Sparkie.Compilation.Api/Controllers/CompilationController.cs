using Microsoft.AspNetCore.Mvc;
using We.Sparkie.Compilation.Api.Repository;

namespace We.Sparkie.Compilation.Api.Controllers
{
    public class CompilationController : EntityController<Entities.Compilation>
    {
        public CompilationController(Repository<Entities.Compilation> repository) : base(repository)
        {
        }
    }
}