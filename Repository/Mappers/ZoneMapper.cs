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

            return new Zone(
               id: reader.GetInt32(reader.GetOrdinal("Id")),
               name: reader.GetString(reader.GetOrdinal("Name")),
               description: reader.GetString(reader.GetOrdinal("Description"))
           );
        }
    }
}
