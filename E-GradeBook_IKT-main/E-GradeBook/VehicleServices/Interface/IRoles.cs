using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleServices.Interface
{
    public interface IRoles
    {
        bool check(string userId);
        bool checkProfessor(string userId);
        List<ApplicationUser> getStudents();
        List<ApplicationUser> getProfesors();
        void postProfessor(string Id);
        void deleteProfessor(string Id);
        ApplicationUser getWantedUser(string userId);
        ApplicationUser find(string email);
    }
}
