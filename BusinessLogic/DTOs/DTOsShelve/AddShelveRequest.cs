using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class AddShelveRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int WarehouseId { get; set; }
    }
}
