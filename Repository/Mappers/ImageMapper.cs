// Repository/Mappers/ImageMapper.cs (no BusinessLogic/Común/Mappers)
using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mappers
{
    public static class ImageMapper
    {
        public static Image FromReader(SqlDataReader reader)
        {
            return new Image(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                entityType: reader.GetString(reader.GetOrdinal("EntityType")),
                entityId: reader.GetInt32(reader.GetOrdinal("EntityId")),
                fileName: reader.GetString(reader.GetOrdinal("FileName")),
                blobName: reader.GetString(reader.GetOrdinal("BlobName")),
                contentType: reader.GetString(reader.GetOrdinal("ContentType")),
                size: reader.GetInt64(reader.GetOrdinal("Size")),
                uploadDate: reader.GetDateTime(reader.GetOrdinal("UploadDate"))
            );
        }
    }
}