using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsPayment
{
    public  class DeletePaymentRequest: DeleteRequest
    {
        public int Id { get; set; }
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();
    }
}
