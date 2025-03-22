using Domain;
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
        Subject GetDetailsForSubject(Guid? id);
        void CreateNewSubject(Subject p);
        void UpdateExistingSubject(Subject p);
        void DeleteSubject(Guid id);
    }
}
