using CommonLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.Portal;

namespace ERPTestAPI.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IERPUnitOfWork eRPUnitOfWork) : base(eRPUnitOfWork)
        {

        }

        [HttpGet]
        public BaseResponse Get()
        {
            return constructResponse(eRPUnitOfWork.AppUsersRepository.GetUsers());
        }

       
        [HttpGet]
        [Route("GetUserPersonalSettings")]

        public async Task<BaseResponse> GetUserPersonalSettings()
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.GetUserPersonalSettings());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UserToRole")]
        public async Task<BaseResponse> AddUserToRole([FromBody] UserRoleDto model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.AddUserToRole(model));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddUserClaims")]
        public async Task<BaseResponse> AddUserClaims([FromBody] UserClaimDto model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.AddUserClaims(model));
        }



        [HttpDelete("{id}")]
        public async Task<BaseResponse> Delete(string id)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.SoftDelete(id));
        }
    }
}