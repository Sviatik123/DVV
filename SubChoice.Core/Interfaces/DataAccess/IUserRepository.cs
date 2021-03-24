using System;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
    }
}
