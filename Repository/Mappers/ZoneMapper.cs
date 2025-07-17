using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mappers
{
    public static class ZoneMapper
    {
        public static Zone FromReader(SqlDataReader reader)
        {
            var zone = new Zone(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                deliveryDays: new List<Day>(),
                requestDays: new List<Day>()
            );

            if (!reader.IsDBNull(reader.GetOrdinal("BlobName")))
            {
                var blobName = reader.GetString(reader.GetOrdinal("BlobName"));
                zone.ImageUrl = $"https://chriska.blob.core.windows.net/images/{blobName}";
            }

            
            if (!reader.IsDBNull(reader.GetOrdinal("DeliveryDays")))
            {
                var deliveryDaysStr = reader.GetString(reader.GetOrdinal("DeliveryDays"));
                zone.DeliveryDays = deliveryDaysStr
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim()))
                    .ToList();
            }

           
            if (!reader.IsDBNull(reader.GetOrdinal("RequestDays")))
            {
                var requestDaysStr = reader.GetString(reader.GetOrdinal("RequestDays"));
                zone.RequestDays = requestDaysStr
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim()))
                    .ToList();
            }

            return zone;
        }

    }
}
