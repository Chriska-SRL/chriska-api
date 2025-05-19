using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class ReturnRequest: Request
    {
        public CreditNote CreditNote { get; set; } 
    }
}
