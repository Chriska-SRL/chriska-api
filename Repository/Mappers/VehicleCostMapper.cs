using BusinessLogic.Domain;
using BusinessLogic.Common.Enums;
using Microsoft.Data.SqlClient;
using BusinessLogic.Common;

namespace Repository.Mappers
{
    public static class VehicleCostMapper
    {
        public static VehicleCost FromReader(SqlDataReader reader)
        {

            var vehicle = new Vehicle(
               id: reader.GetInt32(reader.GetOrdinal("VehicleId")),
               plate: reader.GetString(reader.GetOrdinal("Plate")),
               brand: reader.GetString(reader.GetOrdinal("Brand")),
               model: reader.GetString(reader.GetOrdinal("Model")),
               crateCapacity: reader.GetInt32(reader.GetOrdinal("CrateCapacity")),
               costs: new List<VehicleCost>(), 
               auditInfo: new AuditInfo()
           );

            return new VehicleCost(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                vehicle:vehicle,
                type: (VehicleCostType)reader.GetInt32(reader.GetOrdinal("Type")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                date: reader.GetDateTime(reader.GetOrdinal("Date")),
                auditInfo: AuditInfoMapper.FromReader(reader)
                );
        }
    }

}
