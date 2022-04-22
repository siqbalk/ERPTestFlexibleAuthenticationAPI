using EntityLayer.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EntityLayer.ERPDbContext.Entities
{
    public  class AppUser :  IdentityUser, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsSignUpCompleted { get; set; }
        public string Address { get; set; }
        public bool MemberStatus { get; set; }
        public string TimeZone { get; set; }
        public int? JobTitleId { get; set; }
        public bool IsDeleted { get; set; }
        public bool RecieveProductEmails { get; set; }
        public bool RecieveNotifications { get; set; }
        public bool IsPersonalSettingsCompleted { get; set; }
        public int ProfileCompletion { get; set; }

        public static explicit operator ClaimsPrincipal(AppUser v)
        {
            throw new NotImplementedException();
        }
    }
}
