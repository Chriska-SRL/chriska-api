using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Zone
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string Description { get; set; }
        private List<Day> Days { get; set; }
    }
}
