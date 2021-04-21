using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;
using System;

namespace SubChoice.Core.Interfaces.DataAccess
{
    public interface IStudentRepository : IGenericRepository<Student, Guid>
    {
    }
}
