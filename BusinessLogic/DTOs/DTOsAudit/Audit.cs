

namespace BusinessLogic.DTOs.DTOsAudit
{
    public class Audit
    {
        public int? UserId { get; set; } = null;
        public DateTime? Date { get; set; } = null;
        public string? Location { get; set; } = null;
        public void SetAudit(int userId)
        {
            UserId = userId;
            Date = DateTime.UtcNow;
        }
    }
}
