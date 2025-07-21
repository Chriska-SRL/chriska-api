using BusinessLogic.Común;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsClient
{
    public class DeleteClientRequest
    {
        public int ClientId { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();
    }
}
