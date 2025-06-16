using BusinessLogic.Común.Enums;

namespace BusinessLogic.Dominio
{
    public class Product : IEntity<Product.UpdatableData>
    {
        public int Id { get; set; }
        public string InternalCode { get; set; } = string.Empty;
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public UnitType UnitType { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public Product(int id, string barcode, string name, decimal price, string image, int stock, string description, UnitType unitType, TemperatureCondition temperatureCondition, string observation, SubCategory subCategory, List<Supplier> suppliers)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Price = price;
            Image = image;
            Stock = stock;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            Observation = observation;
            SubCategory = subCategory ?? throw new ArgumentNullException(nameof(subCategory));
            Suppliers = suppliers ?? new List<Supplier>();

            Validate();
        }
        public Product(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El ID del producto debe ser mayor a cero.");

            Id = id;
            InternalCode = string.Empty;
            Barcode = "0000000000000";
            Name = "-";
            Price = 1;
            Image = string.Empty;
            Stock = 0;
            Description = "-";
            UnitType = UnitType.Unit;
            TemperatureCondition = TemperatureCondition.None;
            Observation = string.Empty;
            SubCategory = new SubCategory(9999, "Temporal", new Category(9999, "Temporal"));
            Suppliers = new List<Supplier>();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Barcode))
                throw new ArgumentNullException(nameof(Barcode), "El código de barras es obligatorio.");
            if (Barcode.Length != 13 || !Barcode.All(char.IsDigit))
                throw new ArgumentException("El código de barras debe tener exactamente 13 dígitos numéricos.", nameof(Barcode));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre es obligatorio.");
            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "El precio debe ser mayor a 0.");

            if (Stock < 0)
                throw new ArgumentOutOfRangeException(nameof(Stock), "El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción es obligatoria.");
            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción no puede superar los 255 caracteres.");

            if (!Enum.IsDefined(typeof(UnitType), UnitType))
                throw new ArgumentOutOfRangeException(nameof(UnitType), "Tipo de unidad inválido.");

            if (!Enum.IsDefined(typeof(TemperatureCondition), TemperatureCondition))
                throw new ArgumentOutOfRangeException(nameof(TemperatureCondition), "Condición de temperatura inválida.");

            if (!string.IsNullOrWhiteSpace(Image) && Image.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Image), "La ruta de imagen no puede superar los 255 caracteres.");

            if (!string.IsNullOrWhiteSpace(Observation) && Observation.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Observation), "La observación no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            if (data.SubCategory == null)
                throw new ArgumentNullException(nameof(data.SubCategory), "La subcategoría es obligatoria.");

            Name = data.Name;
            Barcode = data.Barcode;
            Price = data.Price;
            Image = data.Image;
            Description = data.Description;
            UnitType = data.UnitType;
            TemperatureCondition = data.TemperatureCondition;
            Observation = data.Observation;
            SubCategory = data.SubCategory;

            SetInternalCode();
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; } = string.Empty;
            public string Barcode { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Image { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public UnitType UnitType { get; set; }
            public TemperatureCondition TemperatureCondition { get; set; }
            public string Observation { get; set; } = string.Empty;
            public SubCategory SubCategory { get; set; } = null!;
        }

        public void SetInternalCode()
        {
            if (Id <= 0)
                throw new InvalidOperationException("El ID debe estar asignado antes de generar el código interno.");

            if (SubCategory == null)
                throw new InvalidOperationException("La subcategoría debe estar asignada antes de generar el código interno.");

            InternalCode = $"{SubCategory.Id}{Id:D4}";
        }

        public override string ToString()
        {
            return $"Product(Id: {Id}, Name: {Name}, InternalCode: {InternalCode}, Price: {Price}, Stock: {Stock}, UnitType: {UnitType}, Temperature: {TemperatureCondition}, SubCategory: {SubCategory?.ToString()})";
        }
    }
}
