using BusinessLogic.Común.Audits;
using BusinessLogic.Común;
using BusinessLogic.DTOs.DTOsAudit;

public static class AuditMapper
{
    public static AuditInfo ToDomain(AuditInfoRequest request)
    {
        var audit = new AuditInfo();

        if (request.Created != null)
        {
            audit.CreatedAt = request.Created.Date ?? DateTime.UtcNow;
            audit.CreatedLocation = request.Created.Location;
            audit.CreatedBy = request.Created.UserId;
        }

        if (request.Updated != null)
        {
            audit.UpdatedAt = request.Updated.Date ?? DateTime.UtcNow;
            audit.UpdatedLocation = request.Updated.Location;
            audit.UpdatedBy = request.Updated.UserId;
        }

        if (request.Deleted != null)
        {
            audit.DeletedAt = request.Deleted.Date ?? DateTime.UtcNow;
            audit.DeletedLocation = request.Deleted.Location;
            audit.DeletedBy = request.Deleted.UserId;
        }

        return audit;
    }

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
                    User = null
                },
                Location = audit.CreatedLocation
            },

            Updated = audit.UpdatedAt.HasValue ? new AuditAction
            {
                At = audit.UpdatedAt.Value,
                By = new AuditUser
                {
                    Id = audit.UpdatedBy ?? 0,
                    User = null
                },
                Location = audit.UpdatedLocation
            } : null,

            Deleted = audit.DeletedAt.HasValue ? new AuditAction
            {
                At = audit.DeletedAt.Value,
                By = new AuditUser
                {
                    Id = audit.DeletedBy ?? 0,
                    User = null
                },
                Location = audit.DeletedLocation
            } : null
        };
    }

}
