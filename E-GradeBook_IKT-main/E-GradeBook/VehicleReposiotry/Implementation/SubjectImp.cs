using Domain;
using Microsoft.EntityFrameworkCore;
using Reposiotry.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReposiotry;

namespace Reposiotry.Implementation
{
    public class SubjectImp : ISubjectInt
    {
        private readonly ApplicationDbContext context;
        private DbSet<Subject> entities;
        public SubjectImp(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Subject>();
        }
        public void AddSubject(Subject Subject)
        {
            entities.Add(Subject);
            context.SaveChanges();
        }

        public void DeleteSubject(Subject Subject)
        {
            entities.Remove(Subject);
            context.SaveChanges();
        }

        public List<Subject> GetAllSubjects()
        {
            //return entities.Include(z => z.Professors).Include("SubjectProfessor.Professor").ToList();
           return entities
    .Include(s => s.Professors)
    .ThenInclude(sp => sp.Professor)
    .ToList();
        }

        public Subject GetSubjectById(Guid id)
        {
            return entities.Include(z => z.Professors).Include("Professors.Professor").SingleOrDefaultAsync(x => x.Id == id).Result;
          

        }

        public void UpdateSubject(Subject Subject)
        {
            entities.Update(Subject);

            context.SaveChanges();
        }
    }
}
