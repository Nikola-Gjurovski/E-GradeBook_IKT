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
    public class SubjectProfessorRepository : ISubjectProfessor
    {
        private readonly ApplicationDbContext context;
        private DbSet<SubjectProfessor> entities;
        public SubjectProfessorRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<SubjectProfessor>();
        }
        public void AddSubjectProfessor(SubjectProfessor Subject)
        {
            entities.Add(Subject);
            context.SaveChanges();
        }

        public void DeleteSubject(SubjectProfessor Subject)
        {
            entities.Remove(Subject);
            context.SaveChanges();
        }

        public SubjectProfessor GetById(Guid Id)
        {
            return entities.Include(x => x.ProfessorStudents)                     // Include the SubjectStudent list
           .ThenInclude(ps => ps.Student)
                .SingleOrDefaultAsync(x => x.Id == Id).Result;
        }

        public SubjectProfessor GetSubjectProfessor(string ProfessorId, Guid SubjectId)
        {
            return entities
       .Include(x => x.Professor)
       .Include(x => x.Subject)
       .Include(x => x.ProfessorStudents)                     // Include the SubjectStudent list
           .ThenInclude(ps => ps.Student)                    // Include the Student from each SubjectStudent
       .SingleOrDefaultAsync(x => x.ApplicationUserId == ProfessorId && x.SubjectId == SubjectId).Result;

        }
        public List<SubjectProfessor> GetAllSubjects(string ProfessorId)
        {
            return  entities
       .Include(x => x.Professor)
       .Include(x => x.Subject)
       .Include(x => x.ProfessorStudents)
           .ThenInclude(ps => ps.Student)
       .Where(x => x.ApplicationUserId == ProfessorId)
       .ToList();

        }

        public void Update(SubjectProfessor Subject)
        {
            if (Subject == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(Subject);
            context.SaveChanges();
        }
    }
}
