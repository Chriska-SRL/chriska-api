using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class OrderRequest : Request
    {
       public Order Order { get; set; } 

    }
}
