namespace BusinessLogic.Dominio
{
    public class SubCategory:IEntity<SubCategory.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

        public SubCategory(int id,string name, string description ,Category category)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Description = description;
        }
        public void Validate()
        {
            if (Category == null) throw new Exception("Falta Categoria");
            if (Name == null) throw new Exception("El nombre no puede estar vacío");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
