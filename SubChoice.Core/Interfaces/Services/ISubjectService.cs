using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<Subject> CreateSubject(SubjectData data);

        Task<Subject> UpdateSubject(int id, SubjectData data);

        Task<Subject> DeleteSubject(int id);

        Task<List<Subject>> SelectAllByTeacherId(Guid teacherId);

        Task<List<Subject>> SelectAllByStudentId(Guid studentId);
        Task<List<Subject>> SelectAllSubjects();
        Task<StudentSubject> RegisterStudent(Guid studentId, int subjectId);

        Task<StudentSubject> UnRegisterStudent(Guid studentId, int subjectId);
        Task<Subject> SelectById(int id);
    }
}