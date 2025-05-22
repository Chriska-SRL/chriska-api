using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsUser
{
    public class AddUserRequest
    {
        private string Name { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private bool IsEnabled { get; set; }

    }
}
