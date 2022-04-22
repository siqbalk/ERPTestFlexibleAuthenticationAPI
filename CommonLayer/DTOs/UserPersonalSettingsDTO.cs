using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.DTOs
{
    public class UserPersonalSettingsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? JobTitleId { get; set; }
        public string PhoneNo { get; set; }
        public string TimeZone { get; set; }
        public bool RecieveProductEmails { get; set; }
        public bool RecieveNotifications { get; set; }
        public string ImageURL { get; set; }
        public IFormFile File { get; set; }
    }
}