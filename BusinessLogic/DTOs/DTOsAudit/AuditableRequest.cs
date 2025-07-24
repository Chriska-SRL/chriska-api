using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsAudit
{
    public abstract class AuditableRequest
    {
        public AuditInfoRequest AuditInfo { get; set; } = new AuditInfoRequest();
    }
}
