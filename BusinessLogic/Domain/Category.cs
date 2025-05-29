namespace BusinessLogic.Dominio
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        public void Update(string name)
        {
            Name = name;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("El nombre no puede estar vacio");
        }
    }
}
