using CommonLayer.DTOs;
using EntityLayer.ERPDbContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Infrastructures.Portal
{
    public interface IAppUsersRepo
    {
        Task<bool> CheckIsEmailExist(string Email, bool isFromAdmin = false);
        Task<bool> CheckIsUserEligibleForNewPswd(string userId);
        Task<bool> CreateNewPassword(ResetPasswordDTO model);
        Task<UserPersonalSettingsDTO> GetUserPersonalSettings();
        List<AppUser> GetUsers();
        bool Logout();
        Task<LoginResponseDTO> ProcessLogin(LoginDTO model);
        Task<bool> RegisterUserWithClaims(RegisterDTO model);
        Task<bool> AddUserClaims(UserClaimDto model);
        Task<bool> AddUserToRole(UserRoleDto model);
        Task<RegisterDTO> RegisterUsingEmail(RegisterDTO model);
        Task<bool> ResendEmailConfirmationTokenMail(RegisterDTO model);
        Task<bool> ResetPassword(ResetPasswordDTO model);
        Task<bool> ResetPasswordWithToken(ResetPasswordDTO model);
        Task<bool> SoftDelete(string id);
        Task<bool> UpdateUser(UpdateUserDTO model);
        Task<RegisterDTO> VerifyEmail(string userId, string token, string planId);
    }
}