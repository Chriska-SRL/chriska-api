using BusinessLogic.DTOs.DTOsSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class PurchaseResponse
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public SupplierResponse Supplier { get; set; }
    }
}
