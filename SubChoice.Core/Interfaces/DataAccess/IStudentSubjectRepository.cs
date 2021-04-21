using System;
using System.Linq;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IStudentSubjectRepository
    {
        IQueryable<StudentSubject> SelectAll(bool isTrackable = false);

        StudentSubject SelectById(Guid studentId, int subjectId);

        StudentSubject Create(StudentSubject data);

        StudentSubject Update(StudentSubject data);

        StudentSubject Delete(Guid studentId, int subjectId);

        void Save();
    }
}
