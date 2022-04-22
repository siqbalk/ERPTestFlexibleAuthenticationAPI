using AutoMapper;
using CommonLayer.DTOs;
using CommonLayer.Helpers;
using EntityLayer.ERPDbContext;
using EntityLayer.ERPDbContext.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Infrastructures.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static CommonLayer.Constants;

namespace RepositoryLayer.Repos.Portal
{
    public class AppUsersRepo : RepositoryBase<AppUser> , IAppUsersRepo
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUsersRepo(IServiceProvider serviceProvider, ERPTestDbCOntext context  , IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponseDTO> ProcessLogin(LoginDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var _signInManager = _serviceProvider.GetRequiredService<SignInManager<AppUser>>();
            SignInResult signIn;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);


                var validatr = _userManager.CheckPasswordAsync(user, model.Password);
                signIn = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
             
                
                if (signIn.Succeeded)
                {
                    var _tokenService = _serviceProvider.GetRequiredService<ITokenService>();
                    var _loginRepo = _serviceProvider.GetRequiredService<ILoginsRepo>();

                    var claims = new List<Claim>()
                    {
                        new Claim("UserId", user.Id),
                        new Claim("UserName",user.UserName),
                    };



                    if (userRoles.FirstOrDefault() != null)
                    {
                        claims.Add(new Claim("Role", userRoles.FirstOrDefault()));

                        var role = await _roleManager.FindByNameAsync(userRoles.FirstOrDefault());
                        var claimsList = await _roleManager.GetClaimsAsync(role);
                        claims.AddRange(claimsList.ToList());

                        foreach (var item in userRoles)
                            claims.Add(new Claim(ClaimTypes.Role, item));
                    }


                    var userClaims = await _userManager.GetClaimsAsync(user);
                    claims.AddRange(userClaims);

                    var accessToken = _tokenService.GenerateAccessToken(claims);
                    var refreshToken = _tokenService.GenerateRefreshToken();


                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    var principal = new ClaimsPrincipal(identity);


                    // cookies signIN
                    await _httpContextAccessor.HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });

                    await _loginRepo.Post(new Login()
                    {
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(10),
                        UserId = user.Id,
                        CreatedBy = user.Id,
                    }, true);

                    OtherConstants.isSuccessful = true;

                    return new LoginResponseDTO()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    };
                }
                else if (signIn.IsLockedOut)
                {
                    OtherConstants.messageType = MessageType.Error;
                    OtherConstants.isSuccessful = false;
                    OtherConstants.responseMsg = "Too many invalid attempts! Account locked out for 5 minutes";
                    return null;
                }
                else
                {
                    OtherConstants.messageType = MessageType.Error;
                    OtherConstants.isSuccessful = false;
                    OtherConstants.responseMsg = "Invalid username or password!";
                    return null;
                }
            }
            else
            {
                OtherConstants.messageType = MessageType.Error;
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "Invalid username or password!";
                return null;
            }
        }

        public bool Logout()
        {
            var context = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var _loginRepo = _serviceProvider.GetRequiredService<ILoginsRepo>();
            var loginDetail = _loginRepo.Get().IgnoreQueryFilters().Where(p => p.RefreshToken == context.Request.Headers["RefreshToken"].ToString()).FirstOrDefault();
            loginDetail.IsDeleted = true;
            loginDetail.ModifiedBy = Utils.GetUserId(_serviceProvider);
            _loginRepo.Put(loginDetail);

            // signout cookies
             _httpContextAccessor.HttpContext.SignOutAsync();
            OtherConstants.isSuccessful = true;
            return OtherConstants.isSuccessful;
        }


        public List<AppUser> GetUsers()
        {
            return _serviceProvider.GetRequiredService<UserManager<AppUser>>().Users.ToList();
        }


        public async Task<bool> ResetPassword(ResetPasswordDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var user = await _userManager.FindByIdAsync(Utils.GetUserId(_serviceProvider));

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    //if (model.EnableTwoStepVerification)
                    //{
                    //    user.TwoFactorEnabled = model.EnableTwoStepVerification;
                    //    var res = await _userManager.UpdateAsync(user);
                    //    if (res.Succeeded)
                    //        OtherConstants.isSuccessful = true;
                    //    else
                    //        OtherConstants.isSuccessful = false;
                    //}
                    //else
                    //{
                    OtherConstants.isSuccessful = true;
                    //}
                }
                else
                {
                    OtherConstants.responseMsg = result.Errors.FirstOrDefault().Description;
                    OtherConstants.isSuccessful = false;
                }
            }
            else
            {
                OtherConstants.isSuccessful = false;
            }
            return OtherConstants.isSuccessful;
        }

        public async Task<bool> UpdateUser(UpdateUserDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(Utils.GetUserId(_serviceProvider));
            if (user != null)
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;


                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    OtherConstants.isSuccessful = true;
                else
                    OtherConstants.isSuccessful = false;
            }
            else
            {
                OtherConstants.isSuccessful = false;
            }

            return OtherConstants.isSuccessful;
        }

        public async Task<bool> AddUserToRole(UserRoleDto model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if(user != null && role != null)
            {
                  var result=   await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    OtherConstants.isSuccessful = true;
                }
                else
                    OtherConstants.isSuccessful = false;


            }
            return OtherConstants.isSuccessful;
        }
            
        

        public async Task<bool> RegisterUserWithClaims(RegisterDTO model)
        {

            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = new AppUser();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.ProfileCompletion = 50;
            var result = await _userManager.CreateAsync(user , model.Password);
            if (result.Succeeded)
            {
                if(model.Role != null)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                if(model.ClaimType.Count() > 0)
                {
                    foreach (var claim in model.ClaimType)
                    {
                       await _userManager.AddClaimAsync(user ,new Claim(claim.ClaimTypeName, claim.UserClaimVlaue));
                    }
                }
              
                    OtherConstants.isSuccessful = true;
            }
            else
                OtherConstants.isSuccessful = false;

            return OtherConstants.isSuccessful;
        }


        public async Task<bool> AddUserClaims(UserClaimDto model)
        {

            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var user = await _userManager.FindByIdAsync(model.UserId);
           
            if(user != null)
            {
                if (model.ClaimType.Count() > 0)
                {
                    foreach (var claim in model.ClaimType)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(claim.ClaimTypeName, claim.UserClaimVlaue));
                    }
                }

                OtherConstants.isSuccessful = true;
            }
            else
                OtherConstants.isSuccessful = false;

            return OtherConstants.isSuccessful;
        }


        public async Task<bool> CheckIsEmailExist(string Email, bool isFromAdmin = false)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                OtherConstants.isSuccessful = true;
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Roles.Admin))
                {
                    if (user.Email.ToLower() == Email.ToLower() && isFromAdmin)
                        OtherConstants.isSuccessful = true;
                    else
                        OtherConstants.isSuccessful = false;
                }
                else
                    OtherConstants.isSuccessful = false;
            }

            return OtherConstants.isSuccessful;
        }

        public async Task<bool> CheckIsUserEligibleForNewPswd(string userId)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.MemberStatus)
                {
                    if (user.EmailConfirmed == false && user.IsSignUpCompleted == false)
                    {
                        OtherConstants.isSuccessful = true;
                    }
                    else
                    {
                        OtherConstants.isSuccessful = false;
                        OtherConstants.responseMsg = "Password is already created.";
                    }
                }
                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(Roles.Client))
                    {
                        if (user.EmailConfirmed == false && user.IsSignUpCompleted == false)
                        {
                            OtherConstants.isSuccessful = true;
                        }
                        else
                        {
                            OtherConstants.isSuccessful = false;
                            OtherConstants.responseMsg = "Password is already created.";
                        }
                    }
                    else
                    {
                        OtherConstants.isSuccessful = false;
                        OtherConstants.responseMsg = "User status is not active";
                    }
                }
            }
            else
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "User not found.";
            }
            return OtherConstants.isSuccessful;
        }

        public async Task<RegisterDTO> VerifyEmail(string userId, string token, string planId)
        {
            var model = new RegisterDTO();
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !string.IsNullOrWhiteSpace(token))
            {
                if (user.EmailConfirmed)
                {
                    if (user.IsSignUpCompleted)
                        model.IsSignupCompleted = true;
                    model.IsEmailVerified = true;
                    model.Email = user.Email;
                    OtherConstants.responseMsg = "Email verified successfully.";
                    OtherConstants.isSuccessful = true;
                }
                else if (user.IsSignUpCompleted)
                {
                    model.IsEmailVerified = true;
                    model.IsSignupCompleted = true;
                    OtherConstants.isSuccessful = false;
                }
                else
                {
                    token = token.Replace(" ", "+");
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        model.IsEmailVerified = true;
                        model.IsSignupCompleted = false;
                        model.UserId = userId;
                        model.Email = user.Email;
                        OtherConstants.responseMsg = "Email verified successfully.";
                        OtherConstants.isSuccessful = true;
                    }
                    else
                    {
                        OtherConstants.responseMsg = "Error while verification of email";
                        OtherConstants.isSuccessful = false;
                    }
                }
            }
            else
            {
                OtherConstants.responseMsg = "Error while verification of email";
                OtherConstants.isSuccessful = false;
            }

            return model;
        }

        //public async Task<bool> GenerateForgotPasswordToken(string email)
        //{
        //    var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        var currentRoles = await _userManager.GetRolesAsync(user);
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var url = (currentRoles.Contains(Roles.Admin) ? DomainConfiguration.SuperAdminAppDomain : DomainConfiguration.PortalAppDomain) + $"#/auth/new-password?userid={user.Id}&token={token}";
        //        var body = CreateEmailTemplate(url, EmailTemplateConfiguration.ResetEmailDescription, EmailTemplateConfiguration.ResetEmailButtonTitle, EmailTemplateConfiguration.ResetEmailMessage, EmailTemplateConfiguration.ResetEmailAddress);

        //        var isEmailSent = _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(user.Email, "Reset Password", body, true);
        //        if (isEmailSent)
        //            OtherConstants.isSuccessful = true;
        //        else
        //            OtherConstants.isSuccessful = false;
        //    }
        //    else
        //        OtherConstants.isSuccessful = false;

        //    return OtherConstants.isSuccessful;
        //}

        public async Task<bool> ResetPasswordWithToken(ResetPasswordDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user != null && !string.IsNullOrWhiteSpace(model.Token))
            {
                model.Token = model.Token.Replace(" ", "+");
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    OtherConstants.responseMsg = "Password Updated Successfully.";
                    OtherConstants.isSuccessful = true;
                }
                else
                {
                    OtherConstants.responseMsg = "Password Reset token expired.";
                    OtherConstants.isSuccessful = false;
                }
            }
            else
            {
                OtherConstants.responseMsg = "Email not found.";
                OtherConstants.isSuccessful = false;
            }

            return OtherConstants.isSuccessful;
        }

        public async Task<bool> CreateNewPassword(ResetPasswordDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (user.IsSignUpCompleted == false && user.EmailConfirmed == false && (user.MemberStatus || roles.Contains(Roles.Client)))
                {
                    var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.IsSignUpCompleted = true;
                        user.EmailConfirmed = true;
                        var res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            OtherConstants.responseMsg = "Password Created Successfully.";
                            OtherConstants.isSuccessful = true;
                        }
                        else
                        {
                            OtherConstants.responseMsg = "Password Creation Failed.";
                            OtherConstants.isSuccessful = false;
                        }
                    }
                    else
                    {
                        OtherConstants.responseMsg = "Password Creation Failed.";
                        OtherConstants.isSuccessful = false;
                    }
                }
                else
                {
                    OtherConstants.responseMsg = "Password is already created.";
                    if (!user.MemberStatus)
                    {
                        OtherConstants.responseMsg = "User status is not active.";
                    }
                    OtherConstants.isSuccessful = false;
                }
            }
            else
            {
                OtherConstants.responseMsg = "User not found.";
                OtherConstants.isSuccessful = false;
            }

            return OtherConstants.isSuccessful;
        }

        public async Task<bool> ResendEmailConfirmationTokenMail(RegisterDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                OtherConstants.isSuccessful = false;
            }
            return OtherConstants.isSuccessful;
        }

        public async Task<RegisterDTO> RegisterUsingEmail(RegisterDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = "",
                LastName = "",
                EmailConfirmed = false,
                MemberStatus = true
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                OtherConstants.isSuccessful = false;
            }
            else
                OtherConstants.isSuccessful = false;

            return new RegisterDTO()
            {
                UserId = user.Id
            };
        }


        //public async Task Update(TeamMemberDTO model)
        //{
        //    var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    user.FirstName = model.FirstName;
        //    user.LastName = model.LastName;
        //    user.Email = model.Email;
        //    user.UserName = model.Email;
        //    user.TenantId = Utils.GetTenantId(_serviceProvider);
        //    if (model.BlobURI != null)
        //    {
        //        user.ImageName = model.ImageName;
        //        user.BlobURI = model.BlobURI;
        //    }
        //    user.TimeZone = model.TimeZone;
        //    user.JobTitleId = model.JobTitleId == 0 ? null : model.JobTitleId;
        //    user.CompensationTypeId = model.CompensationTypeId == 0 ? null : model.CompensationTypeId;
        //    user.CurrencyTypeId = model.CurrencyTypeId == 0 ? null : model.CompensationTypeId;
        //    user.CompensationAmountId = model.CompensationAmountId == 0 ? null : model.CompensationAmountId;
        //    user.CityId = model.CityId;
        //    user.PhoneNumber = model.PhoneNumber;
        //    user.SecondaryPhoneNumber = model.SecondaryPhoneNumber;
        //    user.Address = model.Address;
        //    user.MemberStatus = model.MemberStatus;
        //    var result = await _userManager.UpdateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        var role = await _userManager.GetRolesAsync(user);
        //        await _userManager.RemoveFromRolesAsync(user, role);
        //        await _userManager.AddToRoleAsync(user, model.Role);
        //        user = await _userManager.FindByEmailAsync(model.Email);
        //        var url = $"{DomainConfiguration.PortalAppDomain}#/auth/new-password?userid={user.Id}";
        //        var body = CreateEmailTemplate(url, EmailTemplateConfiguration.VerifyEmailDescription, EmailTemplateConfiguration.VerifyEmailButtonTitle, EmailTemplateConfiguration.VerifyEmailMessage, EmailTemplateConfiguration.VerifyEmailAddress);
        //        if (!string.IsNullOrWhiteSpace(body))
        //        {
        //            if (user.MemberStatus && !user.EmailConfirmed && !user.IsSignUpCompleted)
        //            {
        //                var isEmailSent = _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(user.Email, "Verify Email", body, true);
        //                if (isEmailSent)
        //                    OtherConstants.isSuccessful = true;
        //                else
        //                    OtherConstants.isSuccessful = false;
        //            }
        //            else
        //                OtherConstants.isSuccessful = true;
        //        }
        //        else
        //            OtherConstants.isSuccessful = false;
        //    }
        //    else
        //        OtherConstants.isSuccessful = false;
        //}

        //public async Task Post(TeamMemberDTO model)
        //{
        //    var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        //    var user = new AppUser();
        //    user.FirstName = model.FirstName;
        //    user.LastName = model.LastName;
        //    user.Email = model.Email;
        //    user.UserName = model.Email;
        //    user.TenantId = Utils.GetTenantId(_serviceProvider);
        //    user.ImageName = model.ImageName;
        //    user.BlobURI = model.BlobURI;
        //    user.TimeZone = model.TimeZone;
        //    user.JobTitleId = model.JobTitleId == 0 ? null : model.JobTitleId;
        //    user.CompensationTypeId = model.CompensationTypeId == 0 ? null : model.CompensationTypeId;
        //    user.CurrencyTypeId = model.CurrencyTypeId == 0 ? null : model.CompensationTypeId;
        //    user.CompensationAmountId = model.CompensationAmountId == 0 ? null : model.CompensationAmountId;
        //    user.CityId = model.CityId;
        //    user.PhoneNumber = model.PhoneNumber;
        //    user.SecondaryPhoneNumber = model.SecondaryPhoneNumber;
        //    user.Address = model.Address;
        //    user.MemberStatus = model.MemberStatus;
        //    user.ProfileCompletion = 50;
        //    var result = await _userManager.CreateAsync(user);
        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(user, model.Role);
        //        user = await _userManager.FindByEmailAsync(model.Email);
        //        var url = $"{DomainConfiguration.PortalAppDomain}#/auth/new-password?userid={user.Id}";
        //        var body = CreateEmailTemplate(url, EmailTemplateConfiguration.VerifyEmailDescription, EmailTemplateConfiguration.VerifyEmailButtonTitle, EmailTemplateConfiguration.VerifyEmailMessage, EmailTemplateConfiguration.VerifyEmailAddress);
        //        if (!string.IsNullOrWhiteSpace(body))
        //        {
        //            if (user.MemberStatus)
        //            {
        //                var isEmailSent = _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(user.Email, "Verify Email", body, true);
        //                if (isEmailSent)
        //                    OtherConstants.isSuccessful = true;
        //                else
        //                    OtherConstants.isSuccessful = false;
        //            }
        //            else
        //                OtherConstants.isSuccessful = true;
        //        }
        //        else
        //            OtherConstants.isSuccessful = false;
        //    }
        //    else
        //        OtherConstants.isSuccessful = false;
        //}

        public async Task<bool> SoftDelete(string id)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(id);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            OtherConstants.isSuccessful = true;
            return true;
        }

        public async Task<UserPersonalSettingsDTO> GetUserPersonalSettings()
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await _userManager.FindByIdAsync(Utils.GetUserId(_serviceProvider));
            return _mapper.Map<UserPersonalSettingsDTO>(user);
        }

    }
}