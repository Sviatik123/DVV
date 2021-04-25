using System;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IRepoWrapper : IDisposable
    {
        IUserRepository Users { get; }

        ISubjectRepository Subjects { get; }

        IStudentSubjectRepository StudentSubjects { get; }

        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }

        int SaveChanges();
    }
}
