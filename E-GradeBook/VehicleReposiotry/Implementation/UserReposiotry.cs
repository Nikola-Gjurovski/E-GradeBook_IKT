using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReposiotry.Interface;

namespace VehicleReposiotry.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ApplicationUser>();
        }
        public IEnumerable<ApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public ApplicationUser Get(string id)
        {
            //    return entities.Include(u => u.TeachingSubjects)
            //.ThenInclude(ts => ts.Subject).SingleOrDefault(s => s.Id == id);
            return entities
            .Include(u => u.TeachingSubjects)                                // Include TeachingSubjects
                .ThenInclude(ts => ts.Subject)                              // Include Subject from SubjectProfessor
            .Include(u => u.EnrolledSubjects)                               // Include EnrolledSubjects
                .ThenInclude(es => es.SubjectProfessor)                     // Include SubjectProfessor from SubjectStudent
                    .ThenInclude(sp => sp.Subject)                          // Include Subject from SubjectProfessor
            .Include(u => u.EnrolledSubjects)                               // Again include for SubjectProfessor.Professor
                .ThenInclude(es => es.SubjectProfessor)
                    .ThenInclude(sp => sp.Professor)                        // Include Professor from SubjectProfessor
            .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public ApplicationUser Find(string email)
        {
            return entities.SingleOrDefault(s => s.Email == email);
        }
    }
}
