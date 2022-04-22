using EntityLayer.ERPDbContext;
using EntityLayer.ERPDbContext.Entities;
using RepositoryLayer.Infrastructures.Portal;
using System;

namespace RepositoryLayer.Repos.Portal
{
    public class LoginsRepo : RepositoryBase<Login>, ILoginsRepo
    {
        private readonly IServiceProvider _serviceProvider;

        public LoginsRepo(IServiceProvider serviceProvider, ERPTestDbCOntext context) : base(context)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
