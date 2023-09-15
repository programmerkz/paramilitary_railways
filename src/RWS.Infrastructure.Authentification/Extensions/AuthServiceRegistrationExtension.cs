using AutoMapper;
using RWS.Application.Interfaces;
using RWS.Infrastructure.Authentification.Contexts;
//using RWS.Infrastructure.Authentication.Logic;
using RWS.Infrastructure.Authentification.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RWS.Infrastructure.Authentification.Logics;

namespace RWS.Infrastructure.Authentification.Extensions
{
    public static class AuthServiceRegistrationExtension
    {
        public static void AddAuthenticationService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AuthContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("RwsAuthDbConnectionString"), b =>
                    b.MigrationsAssembly(typeof(AuthContext).Assembly.FullName)));

            service.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                        LifetimeValidator = LifetimeValidator
                    };
                });

            service.AddScoped<IAuthLogic, AuthLogic>();
            service.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}
