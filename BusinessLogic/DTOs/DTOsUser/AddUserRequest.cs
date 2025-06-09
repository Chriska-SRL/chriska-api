using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsUser
{
    public class AddUserRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }

    }
}
