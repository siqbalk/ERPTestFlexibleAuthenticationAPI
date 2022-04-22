using EntityLayer.ERPDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class RoleDTO : BaseDTO
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public int NoOfUsers { get; set; }

        public static implicit operator RoleDTO(AppRole v)
        {
            throw new NotImplementedException();
        }
    }
}
