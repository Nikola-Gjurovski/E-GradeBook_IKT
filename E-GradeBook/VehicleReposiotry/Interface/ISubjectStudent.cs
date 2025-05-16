using Domain;
using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiotry.Interface
{
    public interface ISubjectStudent
    {
        public void AddSubjectStudent(SubjectStudent Subject);

        public void DeleteSubject(SubjectStudent Subject);
        public SubjectStudent GetSubjectStudent(string ProfessorId,Guid subjectProfessor);
        public SubjectStudent GetById(Guid Id);
        public List<ApplicationUser> GetMissingtStudents(Guid subjectProfessor);
    }
}
