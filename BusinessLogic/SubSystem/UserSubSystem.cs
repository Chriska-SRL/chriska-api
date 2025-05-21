using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class UserSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private List<User> Users = new List<User>();
        private List<Permission> Permissions = new List<Permission>();
        private List<Role> Roles = new List<Role>();

        public IUserRepository _userRepository;
        public UserSubSystem(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

    }
}
