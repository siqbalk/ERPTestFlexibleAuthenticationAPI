using EntityLayer.ERPDbContext;
using EntityLayer.ERPDbContext.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitOfWork.DataSeeder
{
    public class Seeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ERPTestDbCOntext  eRPTestDbCOntext;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHostingEnvironment _env;
        private static string createdById;

        public Seeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            eRPTestDbCOntext = _serviceProvider.GetRequiredService<ERPTestDbCOntext>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            _env = _serviceProvider.GetRequiredService<IHostingEnvironment>();
        }

        public async Task Seed()
        {
            await eRPTestDbCOntext.Database.MigrateAsync();

            await AddRoles();
            await AddSuperAdmin();
            await eRPTestDbCOntext.SaveChangesAsync();
        }

        private async Task AddRoles()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                var roles = new List<AppRole>()
                {
                     new AppRole(){ Name = "Admin", Description = "Super User"},
                     new AppRole(){ Name = "CompanyAdmin", Description = "Super User of Company" },
                     new AppRole(){ Name = "User", Description = "Company User" },
                     new AppRole(){ Name = "Client", Description = "Client's of Company Admin" },
                };

                foreach (var item in roles)
                    await _roleManager.CreateAsync(item);
            }
        }

        private async Task AddSuperAdmin()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user = new AppUser()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "superadmin@yopmail.com",
                    UserName = "superadmin",
                    EmailConfirmed = true,
                    MemberStatus = true
                };

                await _userManager.CreateAsync(user, "Test@0000");
                await _userManager.AddToRoleAsync(user, "Admin");

                createdById = _userManager.FindByEmailAsync("superadmin@yopmail.com").Result.Id;
            }
        }

        
    }
}
