using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SubjectProfessor:BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser Professor { get; set; }

        public Guid  SubjectId { get; set; }
        public Subject  Subject { get; set; }
        public ICollection<SubjectStudent>? ProfessorStudents { get; set; }
    }
}
