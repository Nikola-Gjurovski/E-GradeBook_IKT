using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Grades:BaseEntity
    {
        public List<int> ?firsSemester { get; set; }
        public int ?firstSemesterFinal { get; set; }
        public int ?lastSemesterFinal { get;  set; }
        public List<int>? lastSemester { get; set; }
        public int ?finalGrade { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ?Student { get; set; }
        public Guid SubjectId { get; set; }
    }
}
