using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SubjectProfessorsDTO
    {
        public string ProfessorId { get; set; }
        public List<ApplicationUser> professors { get; set; }
        public Guid SubjectId { get;set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Subject Subject {  get;set; }

    }
}
