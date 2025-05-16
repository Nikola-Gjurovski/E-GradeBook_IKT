using Domain;
using Domain.DTO;
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
    public class GradeService : IGradesService
    {
        private readonly IGrades _grades;

     
        public GradeService(IGrades grades)
        {

           _grades = grades;
        }
        public void AddGrades(Grades Grades)
        {
            _grades.AddGrades(Grades);
        }

        public void DeleteGrades(Grades Grades)
        {
            _grades.DeleteGrades(Grades);
        }

        public Grades Find(string ApplicationuserId, Guid SubjectId)
        {
          return _grades.Find(ApplicationuserId, SubjectId);
        }

        public List<Grades> GetAllGradess()
        {
           return _grades.GetAllGradess();
        }

        public Grades GetGradesById(Guid id)
        {
            return _grades.GetGradesById(id);
        }

        public void UpdateGrades(Grades Grades)
        {
            _grades.UpdateGrades(Grades);
        }
       
        public GradesDTO GetGrade(Guid SubjectId,string Userid,int id)
        {
            var model = new GradesDTO();
            model.Id = id;
            model.SubjectId = SubjectId;
            model.ApplicationUserId = Userid;
            return model;
        }
        public void  PostGrade(GradesDTO model)
        {
            var grades = _grades.Find(model.ApplicationUserId, model.SubjectId);
            if (grades != null)
            {
                if (model.Id == 1)
                {
                    if (grades.firsSemester == null)
                    {
                        grades.firsSemester = new List<int>();
                    }
                    grades.firsSemester.Add(model.Grade);
                }
                else if (model.Id == 2)
                {
                    if (grades.lastSemester == null)
                    {
                        grades.lastSemester = new List<int>();
                    }
                    grades.lastSemester.Add(model.Grade);
                }
                else if (model.Id == 3)
                {
                    if (grades.finalGrade == null)
                        grades.finalGrade = 0;
                    grades.finalGrade = model.Grade;
                }
                else if (model.Id == 4)
                {
                    if (grades.firstSemesterFinal == null)
                        grades.firstSemesterFinal = 0;
                    grades.firstSemesterFinal = model.Grade;
                }
                else if (model.Id == 5)
                {
                    if (grades.lastSemesterFinal == null)
                        grades.lastSemesterFinal = 0;
                    grades.lastSemesterFinal = model.Grade;
                }
                _grades.UpdateGrades(grades);
            }
        }
        public void DeleteGrade(Guid SubjectId,string UserId,int Id,int GradeId)
        {
            var grades = _grades.Find(UserId,SubjectId);
            if (Id == 1)
                grades.firsSemester.RemoveAt(GradeId);
            if (Id == 2)
                grades.lastSemester.RemoveAt(GradeId);
            _grades.UpdateGrades(grades);

        }
    }
}
