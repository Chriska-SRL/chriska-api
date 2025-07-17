using BusinessLogic.Común;
using BusinessLogic.Común.Audits;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.Mappers
{
    public static class AuditMapper
    {
        public static AuditInfoResponse ToResponse(AuditInfo audit)
        {
            if (audit == null)
                return new AuditInfoResponse();

            return new AuditInfoResponse
            {
                Created = new AuditAction
                {
                    At = audit.CreatedAt,
                    By = new AuditUser
                    {
                        Id = audit.CreatedBy,
                        User = null 
                    },
                    Location = audit.CreatedLocation
                },
                Updated = audit.UpdatedAt.HasValue ? new AuditAction
                {
                    At = audit.UpdatedAt.Value,
                    By = new AuditUser
                    {
                        Id = audit.UpdatedBy,
                        User = null
                    },
                    Location = audit.UpdatedLocation
                } : null,
                Deleted = audit.DeletedAt.HasValue ? new AuditAction
                {
                    At = audit.DeletedAt.Value,
                    By = new AuditUser
                    {
                        Id = audit.DeletedBy,
                        User = null
                    },
                    Location = audit.DeletedLocation
                } : null
            };
        }
    }
}
