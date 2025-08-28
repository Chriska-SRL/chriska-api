using BusinessLogic.Common.Audits;
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsAudit;

public static class AuditMapper
{

    public static AuditInfoResponse? ToResponse(AuditInfo? audit)
    {
        if(audit == null) return null;
        return new AuditInfoResponse
        {

            Created = audit.CreatedAt.HasValue ? new AuditAction
            {
                At = audit.CreatedAt, 
                By = new AuditUser
                {
                    Id = audit.CreatedBy ?? 0,
                    User = "implementar nombre"
                },
                AuditLocation = audit.CreatedLocation
            }: null,

            Updated = audit.UpdatedAt.HasValue ? new AuditAction
            {
                At = audit.UpdatedAt.Value,
                By = new AuditUser
                {
                    Id = audit.UpdatedBy ?? 0,
                    User = "implementar nombre"
                },
                AuditLocation = audit.UpdatedLocation
            } : null,

            Deleted = audit.DeletedAt.HasValue ? new AuditAction
            {
                At = audit.DeletedAt.Value,
                By = new AuditUser
                {
                    Id = audit.DeletedBy ?? 0,
                    User = "implementar nombre"
                },
                AuditLocation = audit.DeletedLocation
            } : null
        };
    }

}
