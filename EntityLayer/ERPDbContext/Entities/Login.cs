using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.ERPDbContext.Entities
{
    public class Login  :BaseEntity
    {

        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string UserId { get; set; }
    }
}
