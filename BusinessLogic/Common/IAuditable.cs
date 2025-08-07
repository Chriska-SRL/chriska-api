using BusinessLogic.Common;

namespace BusinessLogic.Common
{
    public interface IAuditable
    {
        AuditInfo? AuditInfo { get; set; }
        public void MarkAsDeleted(int? userId, Location? location);
    }
}