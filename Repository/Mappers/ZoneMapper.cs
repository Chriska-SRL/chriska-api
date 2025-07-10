using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ZoneMapper
    {
        public static Zone FromReader(SqlDataReader reader)
        {
            var zone = new Zone
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                DeliveryDays = ParseDays(reader.GetString(reader.GetOrdinal("DeliveryDays"))),
                RequestDays = ParseDays(reader.GetString(reader.GetOrdinal("RequestDays"))),
                Image = reader.GetString(reader.GetOrdinal("Image")),

                // Auditoría de creación
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                CreatedLocation = reader.IsDBNull(reader.GetOrdinal("CreatedLocation")) ? null : reader.GetString(reader.GetOrdinal("CreatedLocation")),

                // Auditoría de actualización
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy")),
                UpdatedLocation = reader.IsDBNull(reader.GetOrdinal("UpdatedLocation")) ? null : reader.GetString(reader.GetOrdinal("UpdatedLocation")),

                // Auditoría de borrado (soft delete)
                DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                DeletedLocation = reader.IsDBNull(reader.GetOrdinal("DeletedLocation")) ? null : reader.GetString(reader.GetOrdinal("DeletedLocation"))
            };

            return zone;
        }

        private static List<Day> ParseDays(string daysString)
        {
            if (string.IsNullOrWhiteSpace(daysString))
                return new List<Day>();

            return daysString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(day => day.Trim())
                .Select(MapSpanishDayToEnum)
                .ToList();
        }

        private static Day MapSpanishDayToEnum(string day)
        {
            return day.ToLowerInvariant() switch
            {
                "lunes" => Day.Monday,
                "martes" => Day.Tuesday,
                "miércoles" or "miercoles" => Day.Wednesday,
                "jueves" => Day.Thursday,
                "viernes" => Day.Friday,
                "sábado" or "sabado" => Day.Saturday,
                "domingo" => Day.Sunday,
                _ => throw new ArgumentException($"Día no reconocido: {day}")
            };
        }
    }
}
