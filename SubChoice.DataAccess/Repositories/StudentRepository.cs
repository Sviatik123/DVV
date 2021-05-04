using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.DataAccess.Repositories
{
    public class StudentRepository: GenericRepository<Student, Guid>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
