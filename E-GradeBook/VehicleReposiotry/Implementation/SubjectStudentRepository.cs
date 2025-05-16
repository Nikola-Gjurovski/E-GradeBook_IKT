using Domain;
using Domain.Identity;
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
    public class SubjectStudentRepository : ISubjectStudent
    {
        private readonly ApplicationDbContext context;
        private DbSet<SubjectStudent> entities;
        private DbSet<ApplicationUser> users;
        public SubjectStudentRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<SubjectStudent>();
            users = context.Set<ApplicationUser>();
        }
        public void AddSubjectStudent(SubjectStudent Subject)
        {
            entities.Add(Subject);
            context.SaveChanges();
        }

        public void DeleteSubject(SubjectStudent Subject)
        {
            entities.Remove(Subject);
            context.SaveChanges();
        }

        public SubjectStudent GetById(Guid Id)
        {
            return entities
       .Include(x => x.SubjectProfessor)
           .ThenInclude(sp => sp.Professor)
       .Include(x => x.SubjectProfessor)
           .ThenInclude(sp => sp.Subject)
       .Include(x => x.Student)
       .SingleOrDefaultAsync(x => x.Id == Id).Result;
           
        }

        public SubjectStudent GetSubjectStudent(string StudentId, Guid subjectProfessor)
        {
            return entities.SingleOrDefaultAsync(x => x.ApplicationUserId == StudentId && x.SubjectProfessorId == subjectProfessor).Result;

        }
        public List<ApplicationUser> GetMissingtStudents( Guid subjectProfessor)
        {
            var list= entities.Where( x=>x.SubjectProfessorId == subjectProfessor).Select(sp => sp.ApplicationUserId).ToList();
            var availableProfessors = users
               .Where(u => u.IsProfessor == false && !list.Contains(u.Id))
               .ToList();

            return availableProfessors;

        }
    }
}
