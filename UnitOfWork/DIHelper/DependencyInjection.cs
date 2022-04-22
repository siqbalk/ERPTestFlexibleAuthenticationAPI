using CommonLayer.Helpers;
using EntityLayer.ERPDbContext;
using EntityLayer.ERPDbContext.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Infrastructures.Portal;
using RepositoryLayer.Repos.Portal;
using System.Net.Http;
using UnitOfWork.DataSeeder;
using UnitOfWork.Portal;

namespace UnitOfWork.DIHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
        {
            //DbContext
            services.AddDbContext<ERPTestDbCOntext>();

            //Identity Service
            services.AddIdentity<AppUser, AppRole>(option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 8;
                    option.Password.RequireNonAlphanumeric = true;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireLowercase = false;
                }).AddEntityFrameworkStores<ERPTestDbCOntext>()
                .AddDefaultTokenProviders();

            //Custom Services
           
            services.AddScoped<ILoginsRepo, LoginsRepo>();
            services.AddScoped<IAppRolesRepo, AppRolesRepo>();
            services.AddTransient<IAppUsersRepo, AppUsersRepo>();
            services.AddScoped<ITokenService, TokenService>();



            //Seeder
            services.AddTransient<Seeder>();

            //Unit Of Work
            services.AddScoped<IERPUnitOfWork, ERPUnitOfWork>();

            return services;
        }
    }
}
