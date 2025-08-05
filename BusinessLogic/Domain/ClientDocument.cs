using BusinessLogic.Common;
using BusinessLogic.Common.Audits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Domain
{
    public class ClientDocument
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observation { get; set; } = string.Empty;
        public AuditInfo auditInfo { get; set; } = new AuditInfo();


    }
}
