namespace BusinessLogic.Dominio
{
    public class Category:IEntity<Category.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("El nombre no puede estar vacio");
        }
        public class UpdatableData
        {
            public string Name { get; set; }
        }
    }
}
