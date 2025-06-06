﻿namespace BusinessLogic.Dominio
{
    public class Product : IEntity<Product.UpdatableData>
    {
        public int Id { get; set; }
        public string InternalCode { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string UnitType { get; set; }
        public string TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public SubCategory SubCategory { get; set; }
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public Product(int id, string internalCode, string barcode, string name, decimal price, string image, int stock, string description, string unitType, string temperatureCondition, string observation, SubCategory subCategory, List<Supplier> suppliers)
        {
            Id = id;
            InternalCode = internalCode;
            Barcode = barcode;
            Name = name;
            Price = price;
            Image = image;
            Stock = stock;
            Description = description;
            UnitType = unitType;
            TemperatureCondition = temperatureCondition;
            Observation = observation;
            SubCategory = subCategory;
            Suppliers = suppliers;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(InternalCode)) throw new Exception("El codigo interno es obligatorio");
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (Price <= 0) throw new Exception("El precio debe ser mayor a 0");
            if (Stock < 0) throw new Exception("El stock no puede ser negativo");
            if (SubCategory == null) throw new Exception("La subcategoria es obligatoria");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
            Price = data.Price;
            Image = data.Image;
            Stock = data.Stock;
            Description = data.Description;
            UnitType = data.UnitType;
            TemperatureCondition = data.TemperatureCondition;
            Observation = data.Observation;
            SubCategory = data.SubCategory;
            Validate();
        }
        public class UpdatableData
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
            public int Stock { get; set; }
            public string Description { get; set; }
            public string UnitType { get; set; }
            public string TemperatureCondition { get; set; }
            public string Observation { get; set; }
            public SubCategory SubCategory { get; set; }
        }
    }
}
