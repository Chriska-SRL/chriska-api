
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsBrand
{
    public class BrandResponse:AuditableResponse
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string  Description { get; set; }
    }
}
