
using BusinessLogic.Common;

namespace BusinessLogic.Común.Audits
{
    public class AuditAction
    {
        public DateTime? At { get; set; }
        public AuditUser? By { get; set; }
        public Location? Location { get; set; }
    }
}