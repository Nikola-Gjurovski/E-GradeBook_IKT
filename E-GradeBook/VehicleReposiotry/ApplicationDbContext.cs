using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace VehicleReposiotry
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Subject> Subjects { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
           
        }
    }
}
