using System;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.DataAccess.Base;

namespace SubChoice.DataAccess.Repositories
{
    public class SubjectRepository : GenericRepository<Subject, int>, ISubjectRepository
    {
        public SubjectRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}
