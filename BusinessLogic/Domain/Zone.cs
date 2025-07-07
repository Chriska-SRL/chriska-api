namespace BusinessLogic.Dominio
{
    public class Zone : IEntity<Zone.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<Day> RequestDays { get; set; } = new List<Day>();
        public List<Day> DeliveryDays { get; set; } = new List<Day>();

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
