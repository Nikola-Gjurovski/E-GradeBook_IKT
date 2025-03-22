using Domain;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReposiotry.Interface;

namespace Services.Implementation
{
    public class SubjectRepository : ISubject
    {
        private readonly IRepository<Subject> _reservationRepository;
        public SubjectRepository(IRepository<Subject> productRepository)
        {
            _reservationRepository = productRepository;
        }
        public void CreateNewSubject(Subject p)
        {
            _reservationRepository.Insert(p);
        }

        public void DeleteSubject(Guid id)
        {
            var r = _reservationRepository.Get(id);
            _reservationRepository.Delete(r);
        }

        public List<Subject> GetAllSubjects()
        {
            return _reservationRepository.GetAll().ToList();
        }

        public Subject GetDetailsForSubject(Guid? id)
        {
            var r = _reservationRepository.Get(id);
            return r;
        }

        public void UpdateExistingSubject(Subject p)
        {
            _reservationRepository.Update(p);
        }
    }
}
