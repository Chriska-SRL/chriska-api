using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class User
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private Boolean isEnabled { get; set; }
        private Role Role { get; set; }
        private List<Request> Requests { get; set; } = new List<Request>();
    }
}
