using CommonLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnitOfWork.Portal;

namespace ERPTestAPI.Controllers
{
    public class RolesController : BaseController
    {
        public RolesController(IERPUnitOfWork  eRPUnitOfWork) : base(eRPUnitOfWork)
        {

        }

        [HttpPost]
        [Route("CreateRoleWithClaims")]
        public async Task<BaseResponse> CreateRoleWithClaims(RoleClaimsDTO roleClaimsDTO)
        {
            return constructResponse(await eRPUnitOfWork.AppRolesRepository.CreateRoleWithClaims(roleClaimsDTO));
        }

        [HttpPut]
        [Route("UpdateRoleWithClaims/{id}")]
        public async Task<BaseResponse> UpdateRoleWithClaims(string id, [FromBody] RoleClaimsDTO roleClaimsDTO)
        {
            return constructResponse(await eRPUnitOfWork.AppRolesRepository.UpdateRoleWithClaims(id, roleClaimsDTO));
        }

        [HttpGet]
        [Route("GetClaimsAgainstRole/{id}")]
        public async Task<BaseResponse> GetClaimsAgainstRole(string id)
        {
            return constructResponse(await eRPUnitOfWork.AppRolesRepository.GetClaimsAgainstRole(id));
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<BaseResponse> GetAllRoles()
        {
            return constructResponse(await eRPUnitOfWork.AppRolesRepository.GetAllRoles());
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse> Delete(string id)
        {
            return constructResponse(await eRPUnitOfWork.AppRolesRepository.SoftDelete(id));
        }

        [HttpGet]
        [Route("CheckRole")]
        public BaseResponse CheckRole(string role)
        {
            return constructResponse(eRPUnitOfWork.AppRolesRepository.CheckIsRoleExist(role));
        }
    }
}
