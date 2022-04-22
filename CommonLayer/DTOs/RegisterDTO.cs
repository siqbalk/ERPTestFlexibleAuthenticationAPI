using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.DTOs
{
    public class RegisterDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
        public string UserId { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsSignupCompleted { get; set; }
        public List<ClaimTypeDTO> ClaimType { get; set; }


    }
}