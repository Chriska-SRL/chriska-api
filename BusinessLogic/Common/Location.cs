using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Common
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location() { }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool IsValid()
        {
            return Latitude >= -90 && Latitude <= 90 &&
                   Longitude >= -180 && Longitude <= 180;
        }

        public override string ToString()
        {
            return $"Lat: {Latitude}, Lng: {Longitude}";
        }

        public static Location? FromString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Espera: "Lat: 10.0, Lng: 20.0"
            var cleaned = value.Replace("Lat:", "").Replace("Lng:", "").Trim();
            var parts = cleaned.Split(',');

            if (parts.Length != 2)
                return null;

            if (!double.TryParse(parts[0].Trim(), out double lat) ||
                !double.TryParse(parts[1].Trim(), out double lng))
                return null;

            var location = new Location(lat, lng);
            return location.IsValid() ? location : null;
        }

    }

}
