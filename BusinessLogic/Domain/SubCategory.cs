using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

        public SubCategory(string name, Category category)
        {

            Name = name;
            Category = category;

        }
        public void Validate()
        {

            if (Category == null) throw new Exception("Falta Categoria");

            if (Name == null) throw new Exception("El nombre no puede estar vacío");

        }

        internal void Update(string name)
        {

            if (string.IsNullOrEmpty(name)) throw new Exception("El nombre no puede estar vacío");
            Name = name;
        }
    }
}
