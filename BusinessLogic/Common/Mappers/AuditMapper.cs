using BusinessLogic.Común.Audits;
using BusinessLogic.Común;
using BusinessLogic.DTOs.DTOsAudit;

public static class AuditMapper
{

    public static AuditInfoResponse ToResponse(AuditInfo audit)
    {

        return new AuditInfoResponse
        {
            Created = new AuditAction
            {
                At = audit.CreatedAt, 
                By = new AuditUser
                {
                    Id = audit.CreatedBy ?? 0,
                    User = "implementar nombre"
                },
                Location = audit.CreatedLocation
            },

            Updated = audit.UpdatedAt.HasValue ? new AuditAction
            {
                At = audit.UpdatedAt.Value,
                By = new AuditUser
                {
                    Id = audit.UpdatedBy ?? 0,
                    User = "implementar nombre"
                },
                Location = audit.UpdatedLocation
            } : null,

            Deleted = audit.DeletedAt.HasValue ? new AuditAction
            {
                At = audit.DeletedAt.Value,
                By = new AuditUser
                {
                    Id = audit.DeletedBy ?? 0,
                    User = "implementar nombre"
                },
                Location = audit.DeletedLocation
            } : null
        };
    }

}
