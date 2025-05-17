using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiotry.Interface
{
    public interface IGrades
    {
        public List<Grades> GetAllGradess();
        public Grades GetGradesById(Guid id);
        public void AddGrades(Grades Grades);
        public void UpdateGrades(Grades Grades);
        public void DeleteGrades(Grades Grades);
        public Grades Find(string ApplicationuserId, Guid SubjectId);
    }
}
