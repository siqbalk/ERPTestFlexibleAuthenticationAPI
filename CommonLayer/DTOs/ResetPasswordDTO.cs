using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.DTOs
{
    public class ResetPasswordDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public bool EnableTwoStepVerification { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
