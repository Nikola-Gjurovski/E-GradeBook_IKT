using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace VehicleReposiotry
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
           
        }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectProfessor> SubjectProfessors { get; set; }

        public virtual DbSet<SubjectStudent> SubjectStudents { get; set; }
        public virtual DbSet<Grades> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SubjectProfessor -> Subject
            modelBuilder.Entity<SubjectProfessor>()
                .HasOne(sp => sp.Subject)
                .WithMany(s => s.Professors)
                .HasForeignKey(sp => sp.SubjectId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade

            // SubjectStudent -> SubjectProfessor
            modelBuilder.Entity<SubjectStudent>()
                .HasOne(ss => ss.SubjectProfessor)
                .WithMany(sp => sp.ProfessorStudents)
                .HasForeignKey(ss => ss.SubjectProfessorId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade

            // SubjectStudent -> ApplicationUser (Student)
            modelBuilder.Entity<SubjectStudent>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.EnrolledSubjects)
                .HasForeignKey(ss => ss.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade

            // SubjectProfessor -> ApplicationUser (Professor)
            modelBuilder.Entity<SubjectProfessor>()
                .HasOne(sp => sp.Professor)
                .WithMany(p => p.TeachingSubjects)
                .HasForeignKey(sp => sp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade
        }


    }
}
