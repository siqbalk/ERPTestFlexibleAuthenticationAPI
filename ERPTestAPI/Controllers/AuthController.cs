using System.Threading.Tasks;
using CommonLayer.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitOfWork.Portal;

namespace ERPTestAPI.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IERPUnitOfWork  eRPUnitOfWork) : base(eRPUnitOfWork)
        {

        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<BaseResponse> Login([FromBody] LoginDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.ProcessLogin(model));
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<BaseResponse> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.ResetPassword(model));
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<BaseResponse> UpdateUser([FromForm] UpdateUserDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.UpdateUser(model));
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<BaseResponse> LogoutAsync()
        {
            eRPUnitOfWork.AppUsersRepository.Logout();
            return constructResponse(await eRPUnitOfWork.Save());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<BaseResponse> Register([FromBody] RegisterDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.RegisterUserWithClaims(model));
        }

    

        [HttpGet]
        [AllowAnonymous]
        [Route("CheckEmail")]
        public async Task<BaseResponse> CheckEmail(string email, bool isFromAdmin = false)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.CheckIsEmailExist(email, isFromAdmin));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CheckIsUserEligibleForNewPswd")]
        public async Task<BaseResponse> CheckIsUserEligibleForNewPswd(string userId)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.CheckIsUserEligibleForNewPswd(userId));
        }

     

        [HttpGet]
        [AllowAnonymous]
        [Route("VerifyEmail")]
        public async Task<BaseResponse> VerifyEmail(string userId, string token, string planId)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.VerifyEmail(userId, token, planId));
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("GenerateForgotPasswordToken")]
        //public async Task<BaseResponse> GenerateForgotPasswordToken(string email)
        //{
        //    return constructResponse(await eRPUnitOfWork.AppUsersRepository.GenerateForgotPasswordToken(email));
        //}

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPasswordWithToken")]
        public async Task<BaseResponse> ResetPasswordWithToken(ResetPasswordDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.ResetPasswordWithToken(model));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateNewPassword")]
        public async Task<BaseResponse> CreateNewPassword(ResetPasswordDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.CreateNewPassword(model));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterUsingEmail")]
        public async Task<BaseResponse> RegisterUsingEmail(RegisterDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.RegisterUsingEmail(model));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResendVerificationEmail")]
        public async Task<BaseResponse> ResendEmailConfirmationTokenMail(RegisterDTO model)
        {
            return constructResponse(await eRPUnitOfWork.AppUsersRepository.ResendEmailConfirmationTokenMail(model));
        }
    }
}
