using BusinessLogic.Común;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve FromReader(SqlDataReader reader)
        {
            return new Shelve(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                warehouse: WarehouseMapper.FromReaderForShelves(reader),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }

        public static Shelve FromReaderForWarehouses(SqlDataReader reader)
        {
            return new Shelve(
                id: reader.GetInt32(reader.GetOrdinal("ShelveId")),
                name: reader.GetString(reader.GetOrdinal("ShelveName")),
                description: reader.GetString(reader.GetOrdinal("ShelveDescription")),
                warehouse: new Warehouse(0),
                auditInfo: new AuditInfo()
            );
        }
    }
}
