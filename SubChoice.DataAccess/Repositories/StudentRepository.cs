using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class StudentRepository: GenericRepository<Student, Guid>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IQueryable<Student> SelectAllWithReferences(bool isTrackable = false)
        {
            return SelectAll(isTrackable).Include(s => s.User).Include(s => s.StudentSubjects);
        }

    }
}
