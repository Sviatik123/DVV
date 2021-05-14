using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Extensions;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubChoice.Services
{
    public class SubjectService : BaseService, ISubjectService
    {
        private IMapper _mapper;
        private IRepoWrapper _repository;

        public SubjectService(IMapper mapper, IRepoWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<Subject>> SelectAllSubjects()
        {
            return await ExecuteAsync(() =>
            {
                var subjects = _repository.Subjects.SelectAll().Include(x => x.Teacher.User).Include(x => x.StudentSubjects);
                return subjects.ToList();
            });
        }

        public async Task<Subject> SelectById(int id)
        {
            return await ExecuteAsync(() =>
            {
                var subject = _repository.Subjects.SelectAll().Where(x => x.Id == id).Include(x => x.Teacher.User).Include(x => x.StudentSubjects);
                return subject.ToList()[0];
            });
        }

        public async Task<Subject> CreateSubject(SubjectData data)
        {
            return await ExecuteAsync(() =>
            {
                var subject = _mapper.Map<SubjectData, Subject>(data);
                var newSubject = _repository.Subjects.Create(subject);
                _repository.SaveChanges();
                return newSubject;
            });
        }

        public async Task<Subject> UpdateSubject(int id, SubjectData data)
        {
            return await ExecuteAsync(() =>
            {
                var subject = _repository.Subjects.SelectById(id);
                subject.MapChanges(data);
                _repository.SaveChanges();
                return subject;
            });
        }

        public async Task<Subject> DeleteSubject(int id)
        {
            return await ExecuteAsync(() =>
            {
                var subject = _repository.Subjects.Delete(id);
                _repository.SaveChanges();
                return subject;
            });
        }

        public async Task<List<Subject>> SelectAllByTeacherId(Guid teacherId)
        {
            return await ExecuteAsync(() =>
            {
                var subjects = _repository.Subjects.SelectAllByTeacherId(teacherId);
                return subjects.ToList();
            });
        }

        public async Task<List<Subject>> SelectAllByStudentId(Guid studentId)
        {
            return await ExecuteAsync(() =>
            {
                var subjects = _repository.Subjects.SelectAllByStudentId(studentId);
                return subjects.ToList();
            });
        }

        public async Task<List<Student>> SelectAllStudentsSubjects(int subjectId)
        {
            return await ExecuteAsync(() =>
            {
                var students = _repository.Students.SelectAll().Include(s => s.StudentSubjects).Where(s => s.StudentSubjects.Any(x => x.SubjectId == subjectId)).Include(x => x.User);
                return students.ToList();
            });
        }

        public async Task<StudentSubject> RegisterStudent(Guid studentId, int subjectId)
        {
            return await ExecuteAsync(() =>
            {
                var studentSubject = new StudentSubject
                {
                    StudentId = studentId,
                    SubjectId = subjectId,
                };
                _repository.StudentSubjects.Create(studentSubject);
                _repository.SaveChanges();
                return studentSubject;
            });
        }

        public async Task<StudentSubject> UnRegisterStudent(Guid studentId, int subjectId)
        {
            return await ExecuteAsync(() =>
            {
                var studentSubject = _repository.StudentSubjects.Delete(studentId, subjectId);
                _repository.SaveChanges();
                return studentSubject;
            });
        }

        public async Task<List<User>> SelectNotApprovedTeachers()
        {
            return await ExecuteAsync(() =>
            {
                var teachers = _repository.Users.SelectAll().Where(t => t.IsApproved == false && t.Teacher != null);
                return teachers.ToList();
            });
        }

        public async Task<User> ApproveUser(Guid id)
        {
            return await ExecuteAsync(() =>
            {
                var user = _repository.Users.SelectById(id);
                ApproveUserDto data = new ApproveUserDto();
                user.MapChanges(data);
                _repository.SaveChanges();
                return user;
            });
        }

        public async Task<bool> CheckIfRecordStudentSubjectExists(int subId, Guid studId)
        {
            return await ExecuteAsync(() =>
            {
                var res = _repository.StudentSubjects.SelectAll().Any(x => x.StudentId == studId && x.SubjectId == subId);
                return res;
            });
        }
    }
}
