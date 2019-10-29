using Microsoft.AspNetCore.Mvc;
using MundoFinanceiro.Shared.Constants;

namespace MundoFinanceiro.Replicacao.Controllers.v1
{
    [ApiController]
    [ApiVersion(VersionConstants.V1)]
    [Route(RouteConstants.ApiRouteTemplate)]
    public class FundamentosController : Controller
    {
        [HttpGet]
        public IActionResult Foo() => Ok("Bar");
    }
}