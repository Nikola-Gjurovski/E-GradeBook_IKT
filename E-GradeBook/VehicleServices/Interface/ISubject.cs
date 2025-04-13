using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ISubject
    {
        List<Subject> GetAllSubjects();
        Subject GetDetailsForSubject(Guid id);
        void CreateNewSubject(Subject p);
        void UpdateExistingSubject(Subject p);
        void DeleteSubject(Guid id);
        SubjectProfessorsDTO GetProfessor(Guid id);
        SubjectProfessorsDTO GetStudent(Guid id);
        public bool PostProfessor(SubjectProfessorsDTO professor);
        public bool PostStudent(SubjectProfessorsDTO professor);
        public void DeleteProfessorSubject(Guid Id);
        public SubjectProfessor GetSubjectProfessor(string ProfessorId, Guid SubjectId);
        public SubjectStudent GetStudentProfessor( Guid SubjectId);
        public void DeleteStudentSubject(Guid Id);
    }
}
