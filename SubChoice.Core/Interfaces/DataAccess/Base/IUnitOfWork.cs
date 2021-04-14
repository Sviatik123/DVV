using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace SubChoice.Core.Interfaces.DataAccess.Base
{
    public interface IUnitOfWork : IDisposable
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();

        Task CreateOrMigrateAsync(bool clean = false);

        IDbContextTransaction BeginTransaction();

        void Commit(IDbContextTransaction transaction);

        void Rollback(IDbContextTransaction transaction);
    }
}
