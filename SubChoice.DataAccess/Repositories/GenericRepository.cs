using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class GenericRepository<TEntity, TEntityIdType> : IGenericRepository<TEntity, TEntityIdType>
         where TEntity : class, IBaseEntity<TEntityIdType>, IIdentifiable<TEntityIdType>, IInactivebleAt, ISaveTrackable
    {
        protected const bool ThrowNotFoundException = false;
        private readonly DbSet<TEntity> _table;


        public GenericRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _table = UnitOfWork.Set<TEntity>();
        }

        protected IQueryable<TEntity> Table => _table.AsNoTracking().Where(x => x.InactiveAt == null).OrderBy(x => x.ModifiedOn).AsQueryable();

        protected IQueryable<TEntity> TableTracking => _table.Where(x => x.InactiveAt == null).OrderBy(x => x.ModifiedOn).AsQueryable();

        protected IUnitOfWork UnitOfWork { get; }

        public IQueryable<TEntity> SelectAll(bool isTrackable = false)
        {
            return isTrackable ? TableTracking : Table;
        }

        public TEntity SelectById(TEntityIdType id, bool throwNotFound = ThrowNotFoundException)
        {
            var entity = _table.Find(id);

            if (entity?.InactiveAt != null)
            {
                entity = null;
            }

            // if (entity == null && throwNotFound)
            // EntityNotFoundException.ThrowMe(typeof(TEntity).Name, nameof(IIdentifiable<TEntityIdType>.Id), id.ToString());
            return entity;
        }

        public TEntity Create(TEntity data)
        {
            var entity = _table.Add(data);
            return entity.Entity;
        }

        public TEntity Update(TEntity data)
        {
            var entity = _table.Attach(data);
            UnitOfWork.Entry(data).State = EntityState.Modified;
            return entity.Entity;
        }

        public TEntity Replace(TEntity entityToReplace, TEntity entity)
        {
            entity.Id = entityToReplace.Id;
            entityToReplace = _table.Attach(entityToReplace).Entity;
            UnitOfWork.Entry(entityToReplace).CurrentValues.SetValues(entity);

            return entityToReplace;
        }

        public TEntity Delete(TEntityIdType id, bool throwNotFound = ThrowNotFoundException)
        {
            var entity = _table.Find(id);

            // if (entity == null && throwNotFound)
            // EntityNotFoundException.ThrowMe(nameof(TEntity), nameof(IIdentifiable<TEntityIdType>.Id), id.ToString());
            _table.Remove(entity);

            return entity;
        }

        public IQueryable<TEntity> SelectAllByIds(IEnumerable<TEntityIdType> ids, bool isTrackable = false)
        {
            var entities =
                (isTrackable ? TableTracking : Table).Where(it => it.InactiveAt == null && ids.Contains(it.Id));
            return entities;
        }

        public IQueryable<TEntityIdType> SelectAllIds()
        {
            return Table
                .Where(s => s.InactiveAt == null)
                .Select(s => s.Id);
        }

        public void Save()
        {
            UnitOfWork.SaveChanges();
        }

        protected IQueryable<TTable> GetTable<TTable>()
            where TTable : class, IInactivebleAt, ISaveTrackable
            => UnitOfWork.Set<TTable>().AsNoTracking().Where(x => x.InactiveAt == null).OrderBy(x => x.ModifiedOn).AsQueryable();
    }
}
