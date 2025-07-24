using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsSupplier
{
    public class AddSupplierRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string RUT { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public int BankAccountId { get; set; }
        public string Observation { get; set; }
    }
}
