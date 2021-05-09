using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;
using System;
using System.Linq;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IStudentRepository : IGenericRepository<Student, Guid>
    {
        IQueryable<Student> SelectAllWithReferences(bool isTrackable = false);
    }
}
