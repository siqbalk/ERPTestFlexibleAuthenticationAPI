using EntityLayer.ERPDbContext;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Infrastructures.Portal;
using System;
using System.Threading.Tasks;
using static CommonLayer.Constants;

namespace UnitOfWork.Portal
{
    public class ERPUnitOfWork : IERPUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ERPTestDbCOntext _context;
        public ERPUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = _serviceProvider.GetRequiredService<ERPTestDbCOntext>();
        }


        public IAppUsersRepo AppUsersRepository => _serviceProvider.GetRequiredService<IAppUsersRepo>();
        public IAppRolesRepo AppRolesRepository => _serviceProvider.GetRequiredService<IAppRolesRepo>();
        public ILoginsRepo LoginsRepository => _serviceProvider.GetRequiredService<ILoginsRepo>();
      


        public async Task<bool> Save()
        {
            try
            {
                if (await _context.SaveChangesAsync() > 0)
                    OtherConstants.isSuccessful = true;
                else
                    OtherConstants.isSuccessful = false;

                return OtherConstants.isSuccessful;
            }
            catch (Exception ex)
            {
                return OtherConstants.isSuccessful;
            }

        }
    }
}
