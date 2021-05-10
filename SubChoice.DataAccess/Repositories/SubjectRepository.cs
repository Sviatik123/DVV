using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class SubjectRepository : GenericRepository<Subject, int>, ISubjectRepository
    {
        public SubjectRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IQueryable<Subject> SelectAllByTeacherId(Guid teacherId, bool isTrackable = false)
        {
            return SelectAll(isTrackable).Where(s => s.TeacherId == teacherId)
                                         .Include(s => s.Teacher)
                                         .Include(s => s.StudentSubjects)
                                         .ThenInclude(ss => ss.Student);
        }

        public IQueryable<Subject> SelectAllByStudentId(Guid studentId, bool isTrackable = false)
        {
            return SelectAll(isTrackable)
                .Include(s => s.Teacher.User)
                .Include(s => s.StudentSubjects)
                .ThenInclude(ss => ss.Student)
                .Where(s => s.StudentSubjects.Select(ss => ss.StudentId).Contains(studentId));
        }
    }
}
