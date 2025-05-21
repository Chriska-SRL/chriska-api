using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> Days { get; set; } = new List<Day>();

        public Zone(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(Description)) throw new Exception("La descripcion es obligatoria");
        }
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }

}
