using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permission> permissions { get; set; } = new List<Permission>();

        public Role(string name)
        {
            Name = name;
        }

        public void Validate()
        {
            if(string.IsNullOrEmpty(Name))
            {
                throw new Exception("El nombre del rol no puede estar vacío");
            }
        }

        public void Update(string roleName)
        {
            Name = roleName;
        }
    }
}
