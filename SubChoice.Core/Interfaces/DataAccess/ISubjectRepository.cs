using System;
using System.Linq;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface ISubjectRepository : IGenericRepository<Subject, int>
    {
        IQueryable<Subject> SelectAllByTeacherId(Guid teacherId, bool isTrackable = false);

        IQueryable<Subject> SelectAllByStudentId(Guid studentId, bool isTrackable = false);
    }
}
