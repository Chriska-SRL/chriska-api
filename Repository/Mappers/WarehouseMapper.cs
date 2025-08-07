using BusinessLogic.Common;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse FromReader(SqlDataReader reader)
        {
            return new Warehouse(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                shelves: new List<Shelve>(),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }

        public static Warehouse FromReaderForShelves(SqlDataReader reader)
        {
            return new Warehouse(
                id: reader.GetInt32(reader.GetOrdinal("WarehouseId")),
                name: reader.GetString(reader.GetOrdinal("WarehouseName")),
                description: reader.GetString(reader.GetOrdinal("WarehouseDescription")),
                shelves: new List<Shelve>(),
                auditInfo: new AuditInfo()
            );
        }
    }
}
