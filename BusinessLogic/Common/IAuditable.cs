using BusinessLogic.Common;

namespace BusinessLogic.Común
{
    public interface IAuditable
    {
        AuditInfo AuditInfo { get; set; }
        public void MarkAsDeleted(int? userId, Location? location);
    }
}