using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class Product : IEntity<Product.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string? Barcode { get; set; }
        public string InternalCode { get; set; } = string.Empty;
        public UnitType UnitType { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public int EstimatedWeight { get; set; } //Peso estimado en gramos
        public int Stock { get; set; }
        public int AvailableStocks { get; set; }
        public string ImageUrl { get; set; } = ""; //url de la imagen
        public string Observation { get; set; }
        public SubCategory? SubCategory { get; set; }
        public Brand? Brand { get; set; }
        public Shelve? Shelve { get; set; }
        //public List<Discount> Discounts { get; set; } = new List<Discount>();
        public List<Supplier>? Suppliers { get; set; } = new List<Supplier>();
        public AuditInfo? AuditInfo { get; set ; } = new AuditInfo();

        public Product( string? barcode, string name, decimal price, string description, UnitType unitType, TemperatureCondition temperatureCondition, int estimatedWeight, string observations, SubCategory subCategory, Brand brand, List<Supplier> suppliers, Shelve shelve)
        {
            Barcode = barcode;
            Name = name;
            Price = price;
            ImageUrl = "";
            Stock = 0;
            AvailableStocks = 0;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            EstimatedWeight = estimatedWeight;
            Observation = observations;
            SubCategory = subCategory ?? throw new ArgumentNullException(nameof(subCategory));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Suppliers = suppliers ?? throw new ArgumentNullException(nameof(suppliers));
            Shelve = shelve ?? throw new ArgumentNullException(nameof(suppliers));
            AuditInfo = new AuditInfo();
            Validate();
        }
        public Product(int id, string? barcode, string name, decimal price, string image, int stock, int availableStocks, string description, UnitType unitType, TemperatureCondition temperatureCondition, int estimatedWeight, string observations, SubCategory? subCategory, Brand? brand,List<Supplier>? suppliers, Shelve? shelve, AuditInfo? auditInfo)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Price = price;
            ImageUrl = image;
            Stock = stock;
            AvailableStocks = availableStocks;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            EstimatedWeight = estimatedWeight;
            Observation = observations;
            SubCategory = subCategory;
            Brand = brand;
            Suppliers = suppliers;
            Shelve = shelve;
            AuditInfo = auditInfo;
            SetInternalCode();
        }
     
        public void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Barcode))
            {
                if (Barcode.Length != 13 || !Barcode.All(char.IsDigit))
                    throw new ArgumentException("El código de barras debe tener exactamente 13 dígitos numéricos si se proporciona.", nameof(Barcode));
            }

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre es obligatorio.");
            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "El precio debe ser mayor a 0.");

            if (Stock < 0)
                throw new ArgumentOutOfRangeException(nameof(Stock), "El stock no puede ser negativo.");

            if (AvailableStocks < 0)
                throw new ArgumentOutOfRangeException(nameof(AvailableStocks), "El stock disponible no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción es obligatoria.");
            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción no puede superar los 255 caracteres.");

            if (!Enum.IsDefined(typeof(UnitType), UnitType))
                throw new ArgumentOutOfRangeException(nameof(UnitType), "Tipo de unidad inválido.");

            if (!Enum.IsDefined(typeof(TemperatureCondition), TemperatureCondition))
                throw new ArgumentOutOfRangeException(nameof(TemperatureCondition), "Condición de temperatura inválida.");

            if (EstimatedWeight < 0)
                throw new ArgumentOutOfRangeException(nameof(EstimatedWeight), "El peso estimado no puede ser negativo.");

            if (!string.IsNullOrWhiteSpace(ImageUrl) && ImageUrl.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(ImageUrl), "La ruta de imagen no puede superar los 255 caracteres.");

            if (!string.IsNullOrWhiteSpace(Observation) && Observation.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Observation), "La observación no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");


            Name = data.Name ?? Name;
            Barcode = data.Barcode ?? Barcode;
            Price = data.Price ?? Price;
            Description = data.Description ?? Description;
            UnitType = data.UnitType ?? UnitType;
            TemperatureCondition = data.TemperatureCondition ?? TemperatureCondition;
            EstimatedWeight = data.EstimatedWeight ?? EstimatedWeight;
            Observation = data.Observation ?? Observation;
            SubCategory = data.SubCategory ?? SubCategory;
            Brand = data.Brand ?? Brand;
            Suppliers = data.Suppliers ?? Suppliers;
            Shelve = data.Shelve ?? Shelve;

            AuditInfo.SetUpdated(data.UserId, data.Location);

            SetInternalCode();
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; } = string.Empty;
            public string? Barcode { get; set; } = string.Empty;
            public decimal? Price { get; set; }
            public string? Description { get; set; } = string.Empty;
            public UnitType? UnitType { get; set; }
            public TemperatureCondition? TemperatureCondition { get; set; }
            public int? EstimatedWeight { get; set; }
            public string? Observation { get; set; } = string.Empty;
            public SubCategory? SubCategory { get; set; } = null!;
            public Brand? Brand { get; set; } = null!;
            public Shelve? Shelve { get; set; } = null!;
            public List<Supplier>? Suppliers { get; set; } = null!;
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

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }
    }
}
