using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class StudentSubjectRepository : IStudentSubjectRepository
    {
        private readonly DbSet<StudentSubject> _table;

        public StudentSubjectRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _table = UnitOfWork.Set<StudentSubject>();
        }

        protected IQueryable<StudentSubject> Table => _table.AsNoTracking().AsQueryable();

        protected IQueryable<StudentSubject> TableTracking => _table.AsQueryable();

        protected IUnitOfWork UnitOfWork { get; }

        public IQueryable<StudentSubject> SelectAll(bool isTrackable = false)
        {
            return isTrackable ? TableTracking : Table;
        }

        public StudentSubject SelectById(Guid studentId, int subjectId)
        {
            var entity = _table.Find(studentId, subjectId);
            return entity;
        }

        public StudentSubject Create(StudentSubject data)
        {
            var entity = _table.Add(data);
            return entity.Entity;
        }

        public StudentSubject Update(StudentSubject data)
        {
            var entity = _table.Attach(data);
            UnitOfWork.Entry(data).State = EntityState.Modified;
            return entity.Entity;
        }

        public StudentSubject Delete(Guid studentId, int subjectId)
        {
            var entity = _table.Find(studentId, subjectId);
            _table.Remove(entity);

            return entity;
        }

        public void Save()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
