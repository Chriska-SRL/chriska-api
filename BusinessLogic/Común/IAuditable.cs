namespace BusinessLogic.Común
{
    public interface IAuditable
    {
        AuditInfo AuditInfo { get; set; }
        void SetDeletedAudit(AuditInfo auditInfo);
    }
}