using BusinessLogic.DTOs.DTOsRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsUser
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }
        public RoleResponse Role { get; set; }
        //public List<RequestResponse> Requests { get; set; } = new List<RequestResponse>();

    }
}
