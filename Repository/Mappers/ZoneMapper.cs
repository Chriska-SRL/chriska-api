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
        // Repository/Mappers/ZoneMapper.cs
        public static Zone FromReader(SqlDataReader reader)
        {
            var zone = new Zone(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description"))
            );

            // Asignar ImageUrl si existe
            if (!reader.IsDBNull(reader.GetOrdinal("BlobName")))
            {
                var blobName = reader.GetString(reader.GetOrdinal("BlobName"));
                zone.ImageUrl = $"https://chriska.blob.core.windows.net/images/{blobName}";
            }

            return zone;
        }
    }
}
