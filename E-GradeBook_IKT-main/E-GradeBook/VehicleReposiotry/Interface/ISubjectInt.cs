using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reposiotry.Interface
{
    public interface ISubjectInt
    {
        public List<Subject> GetAllSubjects();
        public Subject GetSubjectById(Guid id);
        public void AddSubject(Subject Subject);
        public void UpdateSubject(Subject Subject);
        public void DeleteSubject(Subject Subject);
        
    }
}
