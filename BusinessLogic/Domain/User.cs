using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean isEnabled { get; set; }
        public Role Role { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();
    }
}
