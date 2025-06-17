namespace BusinessLogic.Dominio
{
    public class Zone : IEntity<Zone.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> Days { get; set; } = new List<Day>();

        public Zone(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(Description)) throw new Exception("La descripcion es obligatoria");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
            Description = data.Description;
            Validate();
        }
        public class UpdatableData
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
