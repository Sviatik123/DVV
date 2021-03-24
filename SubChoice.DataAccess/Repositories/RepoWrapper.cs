using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SubChoice.Core.Interfaces.DataAccess;

namespace SubChoice.DataAccess.Repositories
{
    public class RepoWrapper : IRepoWrapper
    {
        private readonly DatabaseContext _context;

        public RepoWrapper(DatabaseContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
