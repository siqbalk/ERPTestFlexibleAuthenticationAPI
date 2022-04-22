using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class ClaimTypeDTO
    {
        public string ClaimTypeName { get; set; }
        public ClaimValueDTO ClaimValue { get; set; }
        public string UserClaimVlaue { get; set; }
    }
}
