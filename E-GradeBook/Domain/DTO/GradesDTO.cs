using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class GradesDTO
    {
        public int Grade { get; set; }
        public Guid SubjectId { get; set; }
        public string ApplicationUserId { get; set; }
        public int Id { get; set; }
    }
}
