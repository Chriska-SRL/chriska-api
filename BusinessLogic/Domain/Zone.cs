using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class Zone : IEntity<Zone.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> DeliveryDays { get; set; } = new List<Day>();
        public List<Day> RequestDays { get; set; } = new List<Day>();
        public string? ImageUrl { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Zone(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public Zone(int id)
        {
            Id = id;
            Name = "Temporal";
            Description = "TemporalDesc";
            ImageUrl = "TemporalImage";
        }

        public void SetImageUrl(string? imageUrl)
        {
            ImageUrl = imageUrl;
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
        }
    }
}
