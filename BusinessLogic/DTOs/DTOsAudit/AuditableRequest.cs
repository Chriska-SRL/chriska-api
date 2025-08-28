using BusinessLogic.Common;
namespace BusinessLogic.DTOs.DTOsAudit
{
    public abstract class AuditableRequest
    {
        public Location AuditLocation { get; set; }
        private int? userId;
        public void setUserId (int userId)
        {
            this.userId = userId;
        }

        public int? getUserId()
        {
            return userId;
        }
    }
}
