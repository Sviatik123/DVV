using System;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
