using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class UpdateShelveRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
