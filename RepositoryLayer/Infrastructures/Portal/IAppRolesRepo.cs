using CommonLayer.DTOs;
using EntityLayer.ERPDbContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Infrastructures.Portal
{
    public interface IAppRolesRepo : IRepositoryBase<AppRole>
    {
        Task<bool> CreateRoleWithClaims(RoleClaimsDTO model);
        Task<bool> UpdateRoleWithClaims(string roleId, RoleClaimsDTO model);
        Task<RoleClaimsDTO> GetClaimsAgainstRole(string roleId);
        Task<IEnumerable<AppRole>> GetAllRoles();
        Task<bool> SoftDelete(string id);
        bool CheckIsRoleExist(string roleName);
    }
}
