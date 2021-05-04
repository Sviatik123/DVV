using System.Threading.Tasks;
using SubChoice.Core.Interfaces.DataAccess;

namespace SubChoice.DataAccess.Repositories
{
    public class RepoWrapper : IRepoWrapper
    {
        private readonly DatabaseContext _context;

        public RepoWrapper(DatabaseContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Subjects = new SubjectRepository(_context);
            Students = new StudentRepository(_context);
            Teachers = new TeacherRepository(_context);
            StudentSubjects = new StudentSubjectRepository(_context);
        }

        public IUserRepository Users { get; }

        public ISubjectRepository Subjects { get; }

        public IStudentSubjectRepository StudentSubjects { get; }

        public IStudentRepository Students {get; }

        public ITeacherRepository Teachers { get; }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
