namespace BusinessLogic.Común
{
    public class AuditInfo
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedLocation { get; set; }


        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedLocation { get; set; }


        public DateTime? DeletedAt { get; set; }
        public int DeletedBy { get; set; }
        public string DeletedLocation { get; set; }

        public AuditInfo() { }
    }
}
