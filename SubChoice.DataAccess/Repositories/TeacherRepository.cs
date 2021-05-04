using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.DataAccess.Repositories
{
    public class TeacherRepository: GenericRepository<Teacher, Guid>, ITeacherRepository
    {
        public TeacherRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
