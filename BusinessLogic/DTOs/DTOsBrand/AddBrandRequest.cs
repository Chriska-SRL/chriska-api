using BusinessLogic.Común;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsBrand
{
    public class AddBrandRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    
}
