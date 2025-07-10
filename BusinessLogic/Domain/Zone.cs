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

        public Zone(int id, string name, string description,List<Day> deliveryDays,List<Day> requestDays)
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
