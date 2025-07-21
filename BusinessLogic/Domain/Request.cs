using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public abstract class Request
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        public abstract void Validate();

   
}
}
