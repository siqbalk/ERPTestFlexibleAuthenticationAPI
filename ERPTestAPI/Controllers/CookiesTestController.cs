using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitOfWork.Portal;

namespace ERPTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookiesTestController : BaseController
    {
        public CookiesTestController(IERPUnitOfWork eRPUnitOfWork) : base(eRPUnitOfWork)
        {
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("Authorized Get Action by cookies");
        }
    }
}
