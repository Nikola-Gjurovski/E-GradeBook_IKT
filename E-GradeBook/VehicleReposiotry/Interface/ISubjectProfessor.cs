using Domain;
using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiotry.Interface
{
    public interface ISubjectProfessor
    {
        public void AddSubjectProfessor(SubjectProfessor Subject);
      
        public void DeleteSubject(SubjectProfessor Subject);
        public SubjectProfessor GetSubjectProfessor(string ProfessorId, Guid SubjectId);
        public SubjectProfessor GetById( Guid Id);
        public void Update(SubjectProfessor Subject);
        List<SubjectProfessor> GetAllSubjects(string ProfessorId);
        public List<ApplicationUser> GetAvailableProfessors(Guid subjectId);
    }
}
