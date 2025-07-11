using BusinessLogic.Común.Enums;

namespace BusinessLogic.Dominio
{
    public class Zone : IEntity<Zone.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> DeliveryDays { get; set; } = new List<Day>();
        public List<Day> RequestDays { get; set; } = new List<Day>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public string? CreatedLocation { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedLocation { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedLocation { get; set; }

        public Zone(int id, string name, string description, List<Day> deliveryDays, List<Day> requestDays)
        {
            Id = id;
            Name = name;
            Description = description;
            DeliveryDays = deliveryDays ?? throw new ArgumentNullException(nameof(deliveryDays));
            RequestDays = requestDays ?? throw new ArgumentNullException(nameof(requestDays));

            Validate();
        }
        public Zone(int id)
        {
            Id = id;
            Name = "Temporal";
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
            Validate();
        }
        public class UpdatableData
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public List<Day>? DeliveryDays { get; set; } = new List<Day>();
            public List<Day>? RequestDays { get; set; } = new List<Day>();
        }
    }
}
