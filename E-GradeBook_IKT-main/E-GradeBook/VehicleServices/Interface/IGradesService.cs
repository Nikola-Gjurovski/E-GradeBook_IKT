using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IGradesService
    {
        public List<Grades> GetAllGradess();
        public Grades GetGradesById(Guid id);
        public void AddGrades(Grades Grades);
        public void UpdateGrades(Grades Grades);
        public void DeleteGrades(Grades Grades);
        public void DeleteGrade(Guid SubjectId, string UserId, int Id, int GradeId);
        public Grades Find(string ApplicationuserId, Guid SubjectId);
        public GradesDTO GetGrade(Guid SubjectId, string Userid, int id);
        public void PostGrade(GradesDTO GradesDTO);
    }
}
