// Repository/Mappers/ImageMapper.cs (no BusinessLogic/Común/Mappers)
using BusinessLogic.Domain;
using BusinessLogic.Domain;
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
            throw new NotImplementedException("Image mapping not implemented yet");
        }
    }
}