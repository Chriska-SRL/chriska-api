using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsPurchase
{
    public class AddPurchaseRequest
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();

    }
}
