using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using static CommonLayer.Constants;

namespace CommonLayer.Helpers
{
    public static class ExtensionMethods
    {
    

        public static X509Certificate2 GetCertficateFromStore(this X509Store store, string thumbPrint)
        {
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, false);
            if (cert.Count > 0)
                return cert[0];
            else
                return null;
        }

        public static string GetTenantId(this ClaimsPrincipal principal)
        {
            var data = principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.TenantId);
            if (data != null)
                return data.Value;
            return "";
        }

        public static string GetRole(this ClaimsPrincipal principal)
        {
            var data = principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.Role);
            if (data != null)
                return data.Value;
            return "";
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var data = principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.UserId);
            if (data != null)
                return data.Value;
            return "";
        }
    }
    public static class DbContextExtensions
    {
        public static IQueryable<Object> Set(this DbContext _context, Type t)
        {
            return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
        }

        //public static IQueryable<Object> Set(this DbContext _context, String table)
        //{
        //    Type TableType = _context.GetType().Assembly.GetExportedTypes().FirstOrDefault(t => t.Name == table);
        //    IQueryable<Object> ObjectContext = _context.Set(TableTypeDictionary[table]);
        //    return ObjectContext;
        //}
    }
}
