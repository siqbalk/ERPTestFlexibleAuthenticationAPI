using RepositoryLayer.Infrastructures.Portal;
using System.Threading.Tasks;

namespace UnitOfWork.Portal
{
    public interface IERPUnitOfWork
    {
      
        IAppUsersRepo AppUsersRepository { get; }
        IAppRolesRepo AppRolesRepository { get; }
        ILoginsRepo LoginsRepository { get; }

        Task<bool> Save();
    }
}
