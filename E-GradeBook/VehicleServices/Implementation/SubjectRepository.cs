using Domain;
using Domain.DTO;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Reposiotry.Implementation;
using Reposiotry.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReposiotry.Implementation;
using VehicleReposiotry.Interface;

namespace Services.Implementation
{
    public class SubjectRepository : ISubject
    {
        private readonly ISubjectInt _subjectRepository;
       
        private readonly IUserRepository _userRepository;
        private readonly ISubjectProfessor _subjectProfessor;
        private readonly ISubjectStudent _subjectStudent;
        private readonly IGrades _grades;
        public SubjectRepository(IUserRepository userRepository, ISubjectProfessor subjectProfessor, ISubjectInt subjectRepository, ISubjectStudent subjectStudent, IGrades grades)
        {

            _userRepository = userRepository;
            _subjectProfessor = subjectProfessor;
            _subjectRepository = subjectRepository;
            _subjectStudent = subjectStudent;
            _grades = grades;
        }
        public void CreateNewSubject(Subject p)
        {
             _subjectRepository.AddSubject(p);
        }

       
        public void DeleteSubject(Guid id)
        {
            var subject = _subjectRepository.GetSubjectById(id);
            if (subject == null) return;

            // First delete all related SubjectStudents
            if (subject.Professors != null)
            {
                foreach (var professor in subject.Professors.ToList())
                {
                    if (professor.ProfessorStudents != null)
                    {
                        foreach (var student in professor.ProfessorStudents.ToList())
                        {
                            this.DeleteStudentSubject(student.Id);
                        }
                    }
                }
            }

            // Then delete all SubjectProfessors
            if (subject.Professors != null)
            {
                foreach (var professor in subject.Professors.ToList())
                {
                    this.DeleteProfessorSubject(professor.Id);
                }
            }

            // Finally delete the subject itself
            _subjectRepository.DeleteSubject(subject);
        }

        public List<Subject> GetAllSubjects()
        {
            return  _subjectRepository.GetAllSubjects();
        }

      

        public Subject GetDetailsForSubject(Guid id)
        {
            return _subjectRepository.GetSubjectById(id);
        }

        public SubjectProfessorsDTO GetProfessor(Guid id)
        {
           
            var availableProfessors = _subjectProfessor.GetAvailableProfessors(id);
            var model = new SubjectProfessorsDTO();
            //model.professors = _userRepository.GetAll().Where(x => x.IsProfessor == true).ToList();
            model.professors = availableProfessors;
            model.SubjectId = id;
            model.Subject =  _subjectRepository.GetSubjectById(id);
            
            return model;
        }
        public SubjectProfessorsDTO GetProfessor2(Guid id,string UserId)
        {
            var availableProfessors = _subjectProfessor.GetAvailableProfessors(id);
            var model = new SubjectProfessorsDTO();
            //model.professors = _userRepository.GetAll().Where(x => x.IsProfessor == true).ToList();
            model.professors = availableProfessors;
            model.SubjectId = id;
            model.Subject = _subjectRepository.GetSubjectById(id);
            model.UserId = UserId;

            return model;
        }


        public bool PostProfessor(SubjectProfessorsDTO model)
        {
            var user = _userRepository.Get(model.ProfessorId);
            if(_subjectProfessor.GetSubjectProfessor(model.ProfessorId,model.SubjectId)!=null) {
                return false;
            }
            SubjectProfessor subjectProfessor= new SubjectProfessor();
            subjectProfessor.Id= Guid.NewGuid();
            subjectProfessor.ApplicationUserId =model.ProfessorId;
            subjectProfessor.Professor = user;
            subjectProfessor.SubjectId = model.SubjectId;
            _subjectProfessor.AddSubjectProfessor(subjectProfessor);
            if(user.TeachingSubjects ==  null)
                user.TeachingSubjects = new List<SubjectProfessor>();
            user.TeachingSubjects.Add(subjectProfessor);
            _userRepository.Update(user);
            var subject =  _subjectRepository.GetSubjectById(model.SubjectId);
            if (subject.Professors == null)
                subject.Professors = new List<SubjectProfessor>();
            subject.Professors.Add(subjectProfessor);
             _subjectRepository.UpdateSubject(subject);
            return true;


        }
        public bool UpdateProfessor(SubjectProfessorsDTO model)
        {
            var user = _userRepository.Get(model.ProfessorId);
            SubjectProfessor subjectProfessor = _subjectProfessor.GetSubjectProfessor(model.UserId, model.SubjectId);
            ApplicationUser olduser =_userRepository.Get(subjectProfessor.ApplicationUserId);
           
            subjectProfessor.ApplicationUserId = model.ProfessorId;
            subjectProfessor.Professor = user;
            
            _subjectProfessor.Update(subjectProfessor);
            if (user.TeachingSubjects == null)
                user.TeachingSubjects = new List<SubjectProfessor>();
            user.TeachingSubjects.Add(subjectProfessor);
            _userRepository.Update(user);
           this.UpdateOldUser(olduser, subjectProfessor);
            return true;


        }
        private void UpdateOldUser(ApplicationUser user,SubjectProfessor subjectProfessor)
        {
            user.TeachingSubjects.Remove(subjectProfessor);
            _userRepository.Update(user);
        }


        public void DeleteProfessorSubject(Guid Id)
        {
            var model = _subjectProfessor.GetById(Id);
            if (model == null) return;

            // First delete the dependent entities
            if (model.ProfessorStudents != null)
            {
                foreach (var items in model.ProfessorStudents.ToList()) // ToList() to avoid modification during iteration
                {
                    this.DeleteStudentSubject(items.Id);
                }
            }

            // Then delete the main entity
            _subjectProfessor.DeleteSubject(model);

            // Now update the relationships
            var user = _userRepository.Get(model.ApplicationUserId);
            var subject = _subjectRepository.GetSubjectById(model.SubjectId);

            if (user != null && user.TeachingSubjects!=null && user.TeachingSubjects.Contains(model))
            {
                user.TeachingSubjects.Remove(model);
                _userRepository.Update(user);
            }

            if (subject != null && subject.Professors!=null && subject.Professors.Contains(model))
            {
                subject.Professors.Remove(model);
                _subjectRepository.UpdateSubject(subject);
            }
        }
        public void DeleteStudentSubject(Guid Id)
        {


            var model = _subjectStudent.GetById(Id);

            if (model == null) return;


            var user = _userRepository.Get(model.ApplicationUserId);
            var subject = _subjectProfessor.GetById(model.SubjectProfessorId);

            if (user != null && user.EnrolledSubjects.Contains(model))
            {
                user.EnrolledSubjects.Remove(model);

            }

            if (subject != null && subject.ProfessorStudents.Contains(model))
            {
                subject.ProfessorStudents.Remove(model);

            }


            _subjectStudent.DeleteSubject(model);
            _userRepository.Update(user);
            _subjectProfessor.Update(subject);

            this.DeleteGrades(user,subject.SubjectId);

        }
        protected void DeleteGrades(ApplicationUser user,Guid SubjectId)
        {
            var model = _grades.Find(user.Id, SubjectId);
            user.Grades.Remove(model);
            _grades.DeleteGrades(model);
            _userRepository.Update(user);
        }

        public void UpdateExistingSubject(Subject p)
        {
             _subjectRepository.UpdateSubject(p);
        }

        public SubjectProfessor GetSubjectProfessor(string ProfessorId, Guid SubjectId)
        {
            return _subjectProfessor.GetSubjectProfessor(ProfessorId,SubjectId);
        }

        public SubjectProfessorsDTO GetStudent(Guid id)
        {
            var list = _subjectStudent.GetMissingtStudents(id);
            var model = new SubjectProfessorsDTO();
            //model.professors = _userRepository.GetAll().Where(x => x.IsProfessor == false).ToList();
            model.professors = list;
            model.SubjectId = id;
            model.Id = _subjectProfessor.GetById(id).SubjectId;
            model.UserId = _subjectProfessor.GetById(id).ApplicationUserId;

            
            return model;
        }
        //public bool PostStudent(SubjectProfessorsDTO model)
        //{
        //    var user = _userRepository.Get(model.ProfessorId);
        //    Grades gradesModel = new Grades();

        //    if(_subjectStudent.GetSubjectStudent(model.ProfessorId,model.SubjectId)!=null)
        //    {
        //        return false;
        //    }
        //    SubjectStudent subjectProfessor = new SubjectStudent();
        //    subjectProfessor.Id = Guid.NewGuid();
        //    subjectProfessor.ApplicationUserId = model.ProfessorId;
        //    var subjectP = _subjectProfessor.GetById(model.SubjectId);
        //    subjectProfessor.Student = user;

        //    if (subjectP.ProfessorStudents == null)
        //        subjectP.ProfessorStudents = new List<SubjectStudent>();

        //    subjectP.ProfessorStudents.Add(subjectProfessor);
        //    subjectProfessor.SubjectProfessorId =subjectP.Id;
        //    subjectProfessor.SubjectProfessor = subjectP;
        //    if (user.EnrolledSubjects == null)
        //        user.EnrolledSubjects = new List<SubjectStudent>();
        //    gradesModel.Id = Guid.NewGuid();
        //    gradesModel.SubjectId = subjectP.SubjectId;
        //    gradesModel.ApplicationUserId = user.Id;
        //    gradesModel.Student = user; 
        //    user.EnrolledSubjects.Add(subjectProfessor);
        //    if(user.Grades == null)
        //    {
        //        user.Grades = new List<Grades>();
        //    }
        //    user.Grades.Add(gradesModel);

        //    _subjectStudent.AddSubjectStudent(subjectProfessor);

        //    _userRepository.Update(user);
        //    _subjectProfessor.Update(subjectP);


        //    _grades.AddGrades(gradesModel);
        //    return true;


        //}
        public bool PostStudent(SubjectProfessorsDTO model)
        {
            var user = _userRepository.Get(model.ProfessorId);
            if (_subjectStudent.GetSubjectStudent(model.ProfessorId, model.SubjectId) != null)
            {
                return false;
            }

            var subjectP = _subjectProfessor.GetById(model.SubjectId);

            // Create and setup all entities
            var subjectProfessor = new SubjectStudent
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = model.ProfessorId,
                Student = user,
                SubjectProfessorId = subjectP.Id,
                SubjectProfessor = subjectP
            };

            //var gradesModel = new Grades
            //{
            //    Id = Guid.NewGuid(),
            //    SubjectId = subjectP.SubjectId,
            //    ApplicationUserId = user.Id,
            //    Student = user
            //};

            // Setup collections if null
            subjectP.ProfessorStudents ??= new List<SubjectStudent>();
            user.EnrolledSubjects ??= new List<SubjectStudent>();
            //user.Grades ??= new List<Grades>();

            // Add relationships
            subjectP.ProfessorStudents.Add(subjectProfessor);
            user.EnrolledSubjects.Add(subjectProfessor);
           // user.Grades.Add(gradesModel);

            try
            {
                // Save everything in one operation
                _subjectStudent.AddSubjectStudent(subjectProfessor);
               // _grades.AddGrades(gradesModel);
                _userRepository.Update(user);
                _subjectProfessor.Update(subjectP);


                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                return false;
            }
        }
        public void CreateGrades(SubjectProfessorsDTO model)
        {
          if(_grades.Find(model.ProfessorId,model.SubjectId) !=null)
            {
                return;
            }
            var user = _userRepository.Get(model.ProfessorId);
            var subjectP = _subjectProfessor.GetById(model.SubjectId);
            var gradesModel = new Grades
            {
                Id = Guid.NewGuid(),
                SubjectId = subjectP.SubjectId,
                ApplicationUserId = user.Id,
                Student = user
            };
            if (user.Grades == null)
                user.Grades = new List<Grades>();
            user.Grades.Add(gradesModel);
            _grades.AddGrades(gradesModel);
            _userRepository.Update(user);

        }
        public SubjectStudent GetStudentProfessor(Guid SubjectId)
        {
            return _subjectStudent.GetById(SubjectId);
        }

        public List<SubjectProfessor> GetAllSubjects(string ProfessorId)
        {
            return _subjectProfessor.GetAllSubjects(ProfessorId);
        }
    }
}
