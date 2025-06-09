namespace BusinessLogic.Dominio
{
    public class Category:IEntity<Category.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;  
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("El nombre no puede estar vacio");
        }
        public class UpdatableData
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
