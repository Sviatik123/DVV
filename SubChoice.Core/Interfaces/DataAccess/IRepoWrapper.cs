using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IRepoWrapper : IDisposable
    {
        IUserRepository Users { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
