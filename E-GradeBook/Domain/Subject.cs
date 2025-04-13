using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Subject:BaseEntity
    {
        public string ?Name { get; set; }
        public string ?Description { get; set; }

        public int ?YearOfStudy { get; set; }

        public ICollection<SubjectProfessor> ?Professors { get; set; }
        //public ICollection<SubjectStudent>? Students { get; set; }
    }
}
