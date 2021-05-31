using Microsoft.AspNetCore.Mvc;
using We.Sparkie.Catalogue.Api.Repository;

namespace We.Sparkie.Catalogue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompilationController : EntityController<Entities.Compilation>
    {
        public CompilationController(Repository<Entities.Compilation> repository) : base(repository)
        {
        }
    }
}