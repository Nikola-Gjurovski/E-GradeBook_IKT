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
    public class GradesRepository : IGrades
    {
        private readonly ApplicationDbContext context;
        private DbSet<Grades> entities;
        public GradesRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Grades>();
        }
        public void AddGrades(Grades Grades)
        {
            entities.Add(Grades);
            context.SaveChanges();
        }

        public void DeleteGrades(Grades Grades)
        {
            entities.Remove(Grades);
            context.SaveChanges();
        }

        public Grades Find(string ApplicationuserId, Guid SubjectId)
        {
           return entities.Include(g => g.Student).SingleOrDefaultAsync(x=>x.ApplicationUserId == ApplicationuserId && x.SubjectId == SubjectId).Result;
        }

        public List<Grades> GetAllGradess()
        {
            return entities.ToList();
        }

        public Grades GetGradesById(Guid id)
        {
            return entities.SingleOrDefaultAsync(x => x.Id == id).Result;
        }

        public void UpdateGrades(Grades Grades)
        {
            entities.Update(Grades);

            context.SaveChanges();
        }
    }
}
