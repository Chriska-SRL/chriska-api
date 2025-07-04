﻿using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsCost;
using System.Text.RegularExpressions;

namespace BusinessLogic.Dominio
{
    public class Vehicle : IEntity<Vehicle.UpdatableData>
    {
        public int Id { get; set; }
        public string Plate { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int CrateCapacity { get; private set; }
        public List<VehicleCost> VehicleCosts { get; private set; }

        public Vehicle(int id, string plate, string brand, string model, int crateCapacity, List<VehicleCost> costs)
        {
            Id = id;
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
            VehicleCosts = costs;
            Validate();
        }
        public Vehicle(int id) {
            // Constructor temporal utilizado únicamente para instanciar por Id.
            // No debe usarse para lógica de negocio que requiera datos válidos.
            Id = id;
            Plate = "AAA1234";
            Brand = "Marca Temporal";
            Model = "Modelo Temporal";
            CrateCapacity = 0;
            VehicleCosts = new List<VehicleCost>();
            // No se llama a Validate() porque no se pretende usar la instancia completa
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Plate))
                throw new ArgumentException("La matrícula no puede estar vacía.");

            var plateRegex = new Regex(@"^[A-Z]{3} ?\d{3,4}$");
            if (!plateRegex.IsMatch(Plate))
                throw new ArgumentException("La matrícula tiene un formato inválido. Formato esperado: ABC 1234.");

            if (string.IsNullOrWhiteSpace(Brand))
                throw new ArgumentException("La marca no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(Model))
                throw new ArgumentException("El modelo no puede estar vacío.");

            if (CrateCapacity <= 0)
                throw new ArgumentException("La capacidad de cajones debe ser mayor a cero.");

            if (VehicleCosts == null)
                throw new ArgumentException("La lista de costos no puede ser nula.");

            if (VehicleCosts.Any(c => c == null))
                throw new ArgumentException("La lista de costos no puede contener elementos nulos.");
        }


        public void Update(UpdatableData data)
        {
            Plate = data.Plate ?? Plate;
            Brand = data.Brand ?? Brand;
            Model = data.Model ?? Model;
            CrateCapacity = data.CrateCapacity ?? CrateCapacity;
            Validate();
        }
        public class UpdatableData
        {
            public string? Plate { get; set; }
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public int? CrateCapacity { get; set; }
        }

    }
}
