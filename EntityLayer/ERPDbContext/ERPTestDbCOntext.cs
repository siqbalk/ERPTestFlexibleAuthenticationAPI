using EntityLayer.ERPDbContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static EntityLayer.Helpers.ConnectionStringHelper;

namespace EntityLayer.ERPDbContext
{
    public class ERPTestDbCOntext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ERPTestDbCOntext(DbContextOptions<ERPTestDbCOntext> options) : base(options)
        {
            Database.SetCommandTimeout(120000);
        }

        public DbSet<Login> Logins { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionStrings.CasolvePortalConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }
}

}
