using BusinessLogic.Common;
using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAudit;

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
        public int Stock { get; set; }
        public int AviableStock { get; set; }
        public string Image { get; set; } //url de la imagen
        public string Observation { get; set; }
        public SubCategory SubCategory { get; set; }
        public Brand Brand { get; set; }
        //public List<Discount> Discounts { get; set; } = new List<Discount>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public AuditInfo AuditInfo { get; set ; }

        public Product( string? barcode, string name, decimal price, string image, int stock, int aviableStock, string description, UnitType unitType, TemperatureCondition temperatureCondition, string observations, SubCategory subCategory, Brand brand, List<Supplier> suppliers)
        {
            Barcode = barcode;
            Name = name;
            Price = price;
            Image = image;
            Stock = stock;
            AviableStock = aviableStock;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            Observation = observations;
            SubCategory = subCategory ?? throw new ArgumentNullException(nameof(subCategory));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Suppliers = suppliers ?? throw new ArgumentNullException(nameof(suppliers));
            Validate();
        }
        public Product(int id, string? barcode, string name, decimal price, string image, int stock, int aviableStock, string description, UnitType unitType, TemperatureCondition temperatureCondition, string observations, SubCategory subCategory, Brand brand,List<Supplier> suppliers, AuditInfo auditInfo)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Price = price;
            Image = image;
            Stock = stock;
            AviableStock = aviableStock;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            Observation = observations;
            SubCategory = subCategory ?? throw new ArgumentNullException(nameof(subCategory));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Suppliers = suppliers ?? throw new ArgumentNullException(nameof(suppliers));
            AuditInfo = auditInfo;
            Validate();
        }

        public Product(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El ID del producto debe ser mayor a cero.");

            Id = id;
            InternalCode = "000000";
            Barcode = "0000000000000";
            Name = "Nombre Temporal";
            Price = 1;
            Image = "ImagenTemporal.jpg";
            Stock = 0;
            AviableStock = 0;
            Description = "Descripcion Temporal";
            UnitType = UnitType.Unit;
            TemperatureCondition = TemperatureCondition.None;
            Observation = "Observaciones Temporales";
            SubCategory = new SubCategory(9999);
            Brand = new Brand(9999);
            Suppliers = new List<Supplier>();
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

            if (AviableStock < 0)
                throw new ArgumentOutOfRangeException(nameof(AviableStock), "El stock disponible no puede ser negativo.");

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

            Name = data.Name ?? Name;
            Barcode = data.Barcode ?? Barcode;
            Price = data.Price ?? Price;
            Image = data.Image ?? Image;
            Description = data.Description ?? Description;
            UnitType = data.UnitType ?? UnitType;
            TemperatureCondition = data.TemperatureCondition ?? TemperatureCondition;
            Observation = data.Observation ?? Observation;
            SubCategory = data.SubCategory ?? SubCategory;
            Brand = data.Brand ?? Brand;
            AviableStock = data.AviableStock ?? AviableStock;
            Stock = data.Stock ?? Stock;
            AuditInfo.SetUpdated(data.UserId, data.Location);

            SetInternalCode();
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; } = string.Empty;
            public string? Barcode { get; set; } = string.Empty;
            public decimal? Price { get; set; }
            public string? Image { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
            public UnitType? UnitType { get; set; }
            public TemperatureCondition? TemperatureCondition { get; set; }
            public string? Observation { get; set; } = string.Empty;
            public SubCategory? SubCategory { get; set; } = null!;
            public Brand? Brand { get; set; } = null!;
            public int? Stock { get; set; }
            public int? AviableStock { get; set; }
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
            throw new NotImplementedException();
        }
    }
}
