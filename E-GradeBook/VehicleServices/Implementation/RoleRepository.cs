using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleReposiotry.Interface;
using VehicleServices.Interface;

namespace VehicleServices.Implementation
{
    public class RoleRepository : IRoles
    {
        private readonly IUserRepository _userRepository;
        public RoleRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool check(string userId)
        {
            var loggedUser = _userRepository.Get(userId);
            return loggedUser.IsAdmin==true;
        }

        public void deleteProfessor(string Id)
        {
            var user = _userRepository.Get(Id);
            user.IsProfessor = false;
            _userRepository.Update(user);
        }

        public List<ApplicationUser> getProfesors()
        {
            return _userRepository.GetAll().Where(x => x.IsProfessor == true).ToList();
        }

        public List<ApplicationUser> getStudents()
        {
            return _userRepository.GetAll().Where(x => x.IsProfessor == false).ToList();
        }

        public ApplicationUser getWantedUser(string userId)
        {
            return _userRepository.Get(userId);
        }

        public void postProfessor(string Id)
        {
            var user = _userRepository.Get(Id);
            user.IsProfessor = true;
            _userRepository.Update(user);
        }
    }
}
