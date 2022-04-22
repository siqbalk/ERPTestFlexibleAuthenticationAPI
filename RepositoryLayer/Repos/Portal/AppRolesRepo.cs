using AutoMapper;
using CommonLayer.DTOs;
using CommonLayer.Helpers;
using EntityLayer.ERPDbContext;
using EntityLayer.ERPDbContext.Entities;
using Microsoft.AspNetCore.Identity;
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
    public class AppRolesRepo : RepositoryBase<AppRole>, IAppRolesRepo
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AppRolesRepo(IServiceProvider serviceProvider, ERPTestDbCOntext context) : base(context)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        }
        public async Task<bool> CreateRoleWithClaims(RoleClaimsDTO model)
        {
            var role = _roleManager.Roles.Where(p => p.TenantId == Utils.GetTenantId(_serviceProvider) && p.Name.ToLower() == model.RoleName.Trim().ToLower()).Any();
            if (model.RoleName.Trim().ToLower() == "admin" || model.RoleName.Trim().ToLower() == "companyadmin")
                role = true;
            if (!role)
            {
                AppRole AppRole = new AppRole();
                AppRole.TenantId = Utils.GetTenantId(_serviceProvider);
                AppRole.Name = model.RoleName;
                var result = await _roleManager.CreateAsync(AppRole);
                foreach (var claimType in model.ClaimType)
                {
                    var claims = new List<Claim>();
                    if (claimType.ClaimValue.Create)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Create);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Edit)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Edit);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.View)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.View);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Delete)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Delete);
                        claims.Add(claim);
                    }

                    foreach (var item in claims)
                    {
                        await _roleManager.AddClaimAsync(AppRole, item);
                    }
                }
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "Role already exsists.";
                return false;
            }
        }
        public async Task<bool> UpdateRoleWithClaims(string roleId, RoleClaimsDTO model)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = model.RoleName.Trim();
                role.ModifiedDate = DateTime.UtcNow;
                await _roleManager.UpdateAsync(role);
                var claimsList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimsList)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                foreach (var claimType in model.ClaimType)
                {

                    var claims = new List<Claim>();
                    if (claimType.ClaimValue.Create)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Create);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Edit)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Edit);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.View)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.View);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Delete)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Delete);
                        claims.Add(claim);
                    }

                    foreach (var item in claims)
                    {
                        await _roleManager.AddClaimAsync(role, item);
                    }
                }
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }
        }
        public async Task<IEnumerable<AppRole>> GetAllRoles()
        {
            return  _roleManager.Roles.ToList();
          
        }
        public async Task<RoleClaimsDTO> GetClaimsAgainstRole(string roleId)
        {
            RoleClaimsDTO roleClaimsDTO = new RoleClaimsDTO();
            var role = await _roleManager.FindByIdAsync(roleId);
            roleClaimsDTO.RoleName = role.Name;
            roleClaimsDTO.ClaimType = new List<ClaimTypeDTO>();
            if (role != null)
            {
                var claimsList = await _roleManager.GetClaimsAsync(role);

                foreach (var claimType in Utils.GetClaimTypes())
                {
                    var claims = claimsList.Where(x => x.Type == claimType);
                    roleClaimsDTO.ClaimType.Add(MapClaimTypeWithValues(claims, claimType));

                }
            }
            OtherConstants.isSuccessful = true;
            return roleClaimsDTO;
        }
        private ClaimTypeDTO MapClaimTypeWithValues(IEnumerable<Claim> claims, string claimtype)
        {
            ClaimTypeDTO claimTypeDTO = new ClaimTypeDTO();
            claimTypeDTO.ClaimTypeName = claimtype;
            claimTypeDTO.ClaimValue = new ClaimValueDTO();
            if (claims.Count() > 0)
            {
                foreach (var claim in claims)
                {
                    if (claim.Value == ClaimValue.Create)
                    {
                        claimTypeDTO.ClaimValue.Create = true;
                    }
                    if (claim.Value == ClaimValue.View)
                    {
                        claimTypeDTO.ClaimValue.View = true;
                    }
                    if (claim.Value == ClaimValue.Edit)
                    {
                        claimTypeDTO.ClaimValue.Edit = true;
                    }
                    if (claim.Value == ClaimValue.Delete)
                    {
                        claimTypeDTO.ClaimValue.Delete = true;
                    }
                }
            }
            return claimTypeDTO;
        }
        public async Task<bool> SoftDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            role.IsDeleted = true;
            await _roleManager.UpdateAsync(role);
            OtherConstants.isSuccessful = true;
            return true;
        }

        public bool CheckIsRoleExist(string roleName)
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var isRoleExist = roleManager.Roles.Where(p => p.TenantId == Utils.GetTenantId(_serviceProvider) && p.Name.ToLower() == roleName.ToLower()).Any();
            if (roleName.Trim().ToLower() == "admin" || roleName.Trim().ToLower() == "companyadmin")
                isRoleExist = true;
            if (isRoleExist)
                OtherConstants.isSuccessful = false;
            else
                OtherConstants.isSuccessful = true;

            return OtherConstants.isSuccessful;
        }
    }
}
