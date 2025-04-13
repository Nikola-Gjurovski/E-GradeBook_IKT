using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SubjectStudent:BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser Student { get; set; }

        //public string ProfessorId { get; set; }
        //public ApplicationUser Professor { get; set; }

        //public Guid SubjectId { get; set; }
        //public Subject Subject { get; set; }
        public Guid SubjectProfessorId { get; set; }
        public SubjectProfessor ?SubjectProfessor { get; set; }  
    }
}
