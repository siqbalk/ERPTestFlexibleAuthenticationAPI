using System.Collections.Generic;

namespace CommonLayer.DTOs
{
    public  class UserClaimDto
    {
        public string  UserId { get; set; }
        public List<ClaimTypeDTO> ClaimType { get; set; }
    }
}
