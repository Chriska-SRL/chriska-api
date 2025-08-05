using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Zone : IEntity<Zone.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> DeliveryDays { get; set; } = new List<Day>();
        public List<Day> RequestDays { get; set; } = new List<Day>();
        public List<Client> Clients { get; set; } = new List<Client>();
        public string ImageUrl { get; set; } = "";
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Zone(int id, string name, string description,string image,List<Day> deliveryDays,List<Day> requestDays,AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = image;
            DeliveryDays = deliveryDays;
            RequestDays = requestDays;
            AuditInfo = auditInfo ?? throw new ArgumentNullException(nameof(auditInfo));
            Validate();
        }
        public Zone(string name, string description, List<Day> deliveryDays, List<Day> requestDays)
        {
            Name = name;
            Description = description;
            ImageUrl = "";
            DeliveryDays = deliveryDays;
            RequestDays = requestDays;
            Validate();
        }
        public Zone(int id)
        {
            Id = id;
            Name = "Temporal";
            ImageUrl = "";
            Description = "TemporalDesc";
            DeliveryDays = new List<Day>();
            RequestDays = new List<Day>();
        }


        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(Description)) throw new Exception("La descripcion es obligatoria");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            DeliveryDays = data.DeliveryDays ?? DeliveryDays;
            RequestDays = data.RequestDays ?? RequestDays;
            AuditInfo.SetUpdated(data.UserId, data.Location);

            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public List<Day> DeliveryDays { get; set; }
            public List<Day> RequestDays { get; set; }
        }
    }
}
