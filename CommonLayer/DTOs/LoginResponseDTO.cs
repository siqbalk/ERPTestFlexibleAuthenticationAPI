using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDTO AccountDetails { get; set; }
    }
}
