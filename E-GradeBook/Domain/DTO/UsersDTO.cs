using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UsersDTO
    {
        public string Id { get; set; }
        public List<ApplicationUser> users { get; set; }
    }
}
