using AutoMapper;
using Casolve.Secure.Api.Utilities;
using CommonLayer.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnitOfWork.DataSeeder;
using UnitOfWork.DIHelper;
using static CommonLayer.Constants;
using static EntityLayer.Helpers.ConnectionStringHelper;

namespace ERPTestAPI
{
    public class Startup
    {
        private IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            Utils._config = new ConfigurationBuilder().SetBasePath(_env.ContentRootPath).AddJsonFile("appSettings.json").Build();
            ConnectionStrings.CasolvePortalConnectionString = Utils._config["ConnectionStrings:ERPTestConnectionString"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.RegisterCustomServices();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build();
            });

            services.AddAuthentication()
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
             {
                 o.ExpireTimeSpan = TimeSpan.FromMinutes(30); // optional
             })
             .AddJwtBearer(config =>
             {
                 config.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidIssuer = JWTConfiguration.JWTIssuer,
                     ValidAudience = JWTConfiguration.JWTAudience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfiguration.JWTKey)),
                     ClockSkew = TimeSpan.Zero,
                     LifetimeValidator = TokenLifetimeValidator.Validate
                 };
             })

             .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
             {
                 // runs on each request
                 options.ForwardDefaultSelector = context =>
                 {
                     // filter by auth type
                     string authorization = context.Request.Headers[HeaderNames.Authorization];
                     if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer"))
                         return JwtBearerDefaults.AuthenticationScheme;


                     // otherwise always check for cookie auth
                     return CookieAuthenticationDefaults.AuthenticationScheme;
                 };
             });


            //it will pick up information from both authentication mechanisms
            var multiSchemePolicy = new AuthorizationPolicyBuilder(
       CookieAuthenticationDefaults.AuthenticationScheme,
       JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();

            services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("View", builder => builder.RequireClaim("view", "true"));
                options.AddPolicy("Edit", builder => builder.RequireClaim("edit", "true"));
                options.AddPolicy("Create", builder => builder.RequireClaim("create", "true"));
                options.AddPolicy("Update", builder => builder.RequireClaim("update", "true"));
                options.AddPolicy("Delete", builder => builder.RequireClaim("delete", "true"));
            });

            services.Configure<PasswordHasherOptions>(options =>
       options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
        );



            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });

                    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        Description = "Basic Authorization header using the Bearer scheme."
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                      {
                          {
                                new OpenApiSecurityScheme
                                  {
                                      Reference = new OpenApiReference
                                      {
                                          Type = ReferenceType.SecurityScheme,
                                          Id = "basic"
                                      }
                                  },
                                  new string[] {}
                          }
                      });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Seeder seeder)
        {
            ServiceActivator.Configure(app.ApplicationServices);

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                });
            }
            else
            {
                app.UseHsts();
            }

            seeder.Seed().Wait();

            //app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            List<string> origins = new List<string> { "http://localhost:4200", "https://localhost:4200", "http://localhost:4300", "http://localhost:4500" };

            app.UseCors(options =>
            {
                options.WithOrigins(origins.ToArray()).AllowAnyMethod().AllowCredentials().AllowAnyHeader().SetIsOriginAllowed((host) => true);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

