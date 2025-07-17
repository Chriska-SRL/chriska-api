
namespace BusinessLogic.Común.Audits
{
    public class AuditAction
    {
        public DateTimeOffset At { get; set; }
        public AuditUser? By { get; set; }
        public string? Location { get; set; } = string.Empty;
    }
}