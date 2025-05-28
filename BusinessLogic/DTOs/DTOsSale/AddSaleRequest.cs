using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsSale
{
    public class AddSaleRequest
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }

    }
}
