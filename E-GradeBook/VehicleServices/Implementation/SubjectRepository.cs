using Domain;
using Domain.DTO;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
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
        public SubjectRepository(IUserRepository userRepository, ISubjectProfessor subjectProfessor, ISubjectInt subjectRepository, ISubjectStudent subjectStudent)
        {
            
            _userRepository = userRepository;
            _subjectProfessor = subjectProfessor;
            _subjectRepository = subjectRepository;
            _subjectStudent = subjectStudent;
        }
        public void CreateNewSubject(Subject p)
        {
             _subjectRepository.AddSubject(p);
        }

        //public void DeleteSubject(Guid id)
        //{
        //    var r = _subjectRepository.GetSubjectById(id);
        //    if (r.Professors != null)
        //    {
        //        foreach (var items in r.Professors.ToList()) // ToList() to avoid modification during iteration
        //        {
        //            this.DeleteStudentSubject(items.Id);
        //        }
        //    }
        //    _subjectRepository.DeleteSubject(r);
        //    //var subject = _subjectRepository.GetSubjectById(id);

        //    //if (subject == null) return;

        //    //// Load all SubjectProfessor entries for this subject
        //    //var subjectProfessors = _subjectProfessor.GetAll()
        //    //    .Where(sp => sp.SubjectId == id)
        //    //    .ToList();

        //    //foreach (var sp in subjectProfessors)
        //    //{
        //    //    // Remove SubjectProfessor from Professor's TeachingSubjects
        //    //    var professor = _userRepository.Get(sp.ApplicationUserId);
        //    //    if (professor != null && professor.TeachingSubjects != null)
        //    //    {
        //    //        professor.TeachingSubjects.Remove(sp);
        //    //        _userRepository.Update(professor);
        //    //    }

        //    //    // Remove from subject's Professors list
        //    //    if (subject.Professors != null)
        //    //    {
        //    //        subject.Professors.Remove(sp);
        //    //    }

        //    //    // Delete SubjectProfessor entity
        //    //    _subjectProfessor.DeleteSubject(sp);
        //    //}

        //    //// Finally delete the subject itself
        //    //_subjectRepository.DeleteSubject(subject);
        //}
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
            var model = new SubjectProfessorsDTO();
            model.professors = _userRepository.GetAll().Where(x => x.IsProfessor == true).ToList();
            model.SubjectId = id;
            model.Subject =  _subjectRepository.GetSubjectById(id);
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

        //public void DeleteProfessorSubject(Guid Id)
        //{


        //    var model = _subjectProfessor.GetById(Id);

        //    if (model == null) return;


        //    var user = _userRepository.Get(model.ApplicationUserId);
        //    var subject = _subjectRepository.GetSubjectById(model.SubjectId);

        //    if (user != null && user.TeachingSubjects.Contains(model))
        //    {
        //        user.TeachingSubjects.Remove(model);

        //    }

        //    if (subject != null && subject.Professors.Contains(model))
        //    {
        //        subject.Professors.Remove(model);

        //    }
        //    foreach( var items in model.ProfessorStudents)
        //    {
        //        this.DeleteStudentSubject(items.Id);
        //    }


        //    _subjectProfessor.DeleteSubject(model);
        //    _userRepository.Update(user);
        //    _subjectRepository.UpdateSubject(subject);



        //}
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

            if (user != null && user.TeachingSubjects.Contains(model))
            {
                user.TeachingSubjects.Remove(model);
                _userRepository.Update(user);
            }

            if (subject != null && subject.Professors.Contains(model))
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
            var model = new SubjectProfessorsDTO();
            model.professors = _userRepository.GetAll().Where(x => x.IsProfessor == false).ToList();
            model.SubjectId = id;
            model.Id = _subjectProfessor.GetById(id).SubjectId;
            model.UserId = _subjectProfessor.GetById(id).ApplicationUserId;

            
            return model;
        }
        public bool PostStudent(SubjectProfessorsDTO model)
        {
            var user = _userRepository.Get(model.ProfessorId);
           
            if(_subjectStudent.GetSubjectStudent(model.ProfessorId,model.SubjectId)!=null)
            {
                return false;
            }
            SubjectStudent subjectProfessor = new SubjectStudent();
            subjectProfessor.Id = Guid.NewGuid();
            subjectProfessor.ApplicationUserId = model.ProfessorId;
            var subjectP = _subjectProfessor.GetById(model.SubjectId);
            subjectProfessor.Student = user;
           
            if (subjectP.ProfessorStudents == null)
                subjectP.ProfessorStudents = new List<SubjectStudent>();
           
            subjectP.ProfessorStudents.Add(subjectProfessor);

           
            subjectProfessor.SubjectProfessorId =subjectP.Id;
            subjectProfessor.SubjectProfessor = subjectP;
            if (user.EnrolledSubjects == null)
                user.EnrolledSubjects = new List<SubjectStudent>();
            user.EnrolledSubjects.Add(subjectProfessor);
            _subjectStudent.AddSubjectStudent(subjectProfessor);
            _userRepository.Update(user);
            _subjectProfessor.Update(subjectP);
           


            return true;


        }
        public SubjectStudent GetStudentProfessor(Guid SubjectId)
        {
            return _subjectStudent.GetById(SubjectId);
        }
    }
}
