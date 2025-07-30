using BusinessLogic.Common;

namespace BusinessLogic.Común
{
    public class AuditInfo
    {
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public Location CreatedLocation { get; set; }


        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public Location UpdatedLocation { get; set; }


        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        public Location? DeletedLocation { get; set; }

        public AuditInfo() { }

        public void SetCreated(int? userId, Location? location)
        {
            CreatedAt = DateTime.Now;
            CreatedBy = userId;
            CreatedLocation = location;
        }
        public void SetUpdated(int? userId, Location? location)
        {
            UpdatedAt = DateTime.Now;
            UpdatedBy = userId;
            UpdatedLocation = location;
        }
        public void SetDeleted(int? userId, Location? location)
        {
            DeletedAt = DateTime.Now;
            DeletedBy = userId;
            DeletedLocation = location;
        }
    }
}
