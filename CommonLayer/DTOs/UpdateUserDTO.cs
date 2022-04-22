using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class UpdateUserDTO : UserDTO
    {
        public IFormFile File { get; set; }
    }
}