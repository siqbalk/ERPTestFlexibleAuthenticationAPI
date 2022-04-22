using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class RoleClaimsDTO
    {
        public string RoleName { get; set; }
        public List<ClaimTypeDTO> ClaimType { get; set; }
    }
}
