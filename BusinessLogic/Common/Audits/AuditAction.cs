namespace BusinessLogic.Common.Audits
{
    public class AuditAction
    {
        public DateTime? At { get; set; }
        public AuditUser? By { get; set; }
        public Location? AuditLocation { get; set; }
    }
}