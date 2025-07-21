using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsBrand
{
    public class AddBrandRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
