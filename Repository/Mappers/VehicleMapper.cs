using BusinessLogic.Common;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle FromReader(SqlDataReader reader)
        {
            return new Vehicle(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                plate: reader.GetString(reader.GetOrdinal("Plate")),
                brand: reader.GetString(reader.GetOrdinal("Brand")),
                model: reader.GetString(reader.GetOrdinal("Model")),
                crateCapacity: reader.GetInt32(reader.GetOrdinal("CrateCapacity")),
                costs: new List<VehicleCost>(),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }
    }
}
