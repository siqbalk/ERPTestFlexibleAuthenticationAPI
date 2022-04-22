using CommonLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using UnitOfWork.DIHelper;
using UnitOfWork.Portal;

namespace Casolve.Secure.Api.Utilities
{
    public static class TokenLifetimeValidator
    {
        public static bool Validate(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken tokenToValidate,
            TokenValidationParameters @param
        )
        {
            using var serviceScope = ServiceActivator.GetScope();
            Utils.NewAccessToken = null;
            var context = serviceScope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var _tokenService = serviceScope.ServiceProvider.GetRequiredService<ITokenService>();
            var token = context.Request.Headers["Authorization"].ToString().Substring(7);
            var result = _tokenService.ValidateToken(token);
            if (result)
                return true;
            else
            {
                var _portalUOW = serviceScope.ServiceProvider.GetRequiredService<IERPUnitOfWork>();
                var refreshToken = context.Request.Headers["RefreshToken"].ToString();
                var res = _portalUOW.LoginsRepository.Get().IgnoreQueryFilters().Where(p => p.RefreshToken == refreshToken && p.IsDeleted == false).FirstOrDefault();
                if (res != null)
                {
                    if (res.RefreshTokenExpiryTime > DateTime.UtcNow)
                    {
                        var principals = _tokenService.GetPrincipalFromExpiredToken(token);
                        Utils.NewAccessToken = _tokenService.GenerateAccessToken(principals.Claims);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }
    }
}
