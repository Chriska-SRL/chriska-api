namespace BusinessLogic.Dominio
{
    public class SubCategory:IEntity<SubCategory.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

        public SubCategory(int id,string name, Category category)
        {
            Id = id;
            Name = name;
            Category = category;
        }
        public void Validate()
        {
            if (Category == null) throw new Exception("Falta Categoria");
            if (Name == null) throw new Exception("El nombre no puede estar vacío");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; }
        }
    }
}
