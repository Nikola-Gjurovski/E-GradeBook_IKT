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
    }
}
