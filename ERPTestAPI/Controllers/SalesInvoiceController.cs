using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitOfWork.Portal;

namespace ERPTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesInvoiceController : BaseController
    {
        public SalesInvoiceController(IERPUnitOfWork eRPUnitOfWork) : base(eRPUnitOfWork)
        {
        }

        [HttpGet]
        [Authorize(Policy = "View", Roles = "Admin")]
        public IActionResult Get()
        {
            return Ok("Get/View Action Authorized");
        }


        [HttpPost]
        [Authorize(Policy = "Create", Roles = "Admin")]
        public IActionResult Post()
        {
            return Ok("Create Action Authorized");
        }

        [HttpPut]
        [Authorize(Policy = "Edit", Roles = "Admin")]
        public IActionResult Put()
        {
            return Ok("Edit Action Authorized");
        }

        [HttpDelete]
        [Authorize(Policy = "Delete", Roles = "Admin")]
        public IActionResult Delete()
        {
            return Ok("Delete ActionAuthorized");
        }
    }
}
