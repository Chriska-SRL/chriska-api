namespace BusinessLogic.Dominio
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public List<Shelve> Shelves { get; set; } = new List<Shelve>();

        public Warehouse(string name, string description, string address)
        {
            Name = name;
            Description = description;
            Address = address;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre del almacén es obligatorio");
            if (string.IsNullOrEmpty(Description)) throw new Exception("La descripción del almacén es obligatoria");
            if (string.IsNullOrEmpty(Address)) throw new Exception("La dirección del almacén es obligatoria");
        }

        public void Update(string name, string description, string address)
        {
            Name = name;
            Description = description;
            Address = address;
        }
    }
}
