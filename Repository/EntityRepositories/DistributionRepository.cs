using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using Repository.Utils;

namespace Repository.EntityRepositories
{
    public class DistributionRepository : Repository<Distribution, Distribution.UpdatableData>, IDistributionRepository
    {
        public DistributionRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }


        public async Task<Distribution> AddAsync(Distribution distribution)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Distributions (Observations, Date, UserId, VehicleId) OUTPUT INSERTED.Id VALUES (@Observations, @Date, @UserId, @VehicleId)",
                distribution,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Observations", distribution.Observations);
                    cmd.Parameters.AddWithValue("@Date", distribution.Date);
                    cmd.Parameters.AddWithValue("@UserId", distribution.User.Id);
                    cmd.Parameters.AddWithValue("@VehicleId", distribution.Vehicle.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            await AddDistributionZonesAsync(newId, distribution.Zones);
            await AddDistributionDeliveriesAsync(newId, distribution.DistributionDeliveries);
            distribution.Id = newId;
            return distribution;
        }

        public async Task<Distribution> UpdateAsync(Distribution distribution)
        {
            if (distribution == null)
                throw new ArgumentException(nameof(distribution), "La distribución no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Distributions SET
                    Observations = @Observations,
                    Date         = @Date,
                    UserId       = @UserId,
                    VehicleId    = @VehicleId
                  WHERE Id = @Id",
                distribution,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", distribution.Id);
                    cmd.Parameters.AddWithValue("@Observations", distribution.Observations);
                    cmd.Parameters.AddWithValue("@Date", distribution.Date);
                    cmd.Parameters.AddWithValue("@UserId", distribution.User.Id);
                    cmd.Parameters.AddWithValue("@VehicleId", distribution.Vehicle.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la distribución con Id {distribution.Id}");

            // Reemplazar relaciones
            await DeleteDistributionZonesAsync(distribution.Id);
            await AddDistributionZonesAsync(distribution.Id, distribution.Zones ?? new List<Zone>());

            await DeleteDistributionDeliveriesAsync(distribution.Id);
            await AddDistributionDeliveriesAsync(distribution.Id, distribution.DistributionDeliveries ?? new List<DistributionDelivery>());

            return distribution;
        }


        public async Task<Distribution> DeleteAsync(Distribution distribution)
        {
            if (distribution == null)
                throw new ArgumentException(nameof(distribution), "La distribucion no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Distributions SET IsDeleted = 1 WHERE Id = @Id",
                distribution,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", distribution.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la distribucion con Id {distribution.Id}");

            return distribution;
        }
        string baseQuery = @"
                            SELECT
                                -- Distribution
                                d.Id AS DistributionId,
                                d.Observations,
                                d.Date,
                                d.UserId ,
                                d.VehicleId,

                                -- User (+ Role)
                                u.Name        AS UserName,
                                u.Username      AS UserUsername,
                                u.IsEnabled     AS UserIsEnabled,
                                u.NeedsPasswordChange AS UserNeedsPasswordChange,
                                u.RoleId        AS UserRoleId,

                                r.Id            AS RoleId,
                                r.Name          AS UserRoleName,
                                r.Description   AS UserRoleDescription,

                                -- Vehicle
                                v.Id            AS VehicleId,
                                v.Plate         AS VehiclePlate,
                                v.CrateCapacity AS VehicleCrateCapacity,
                                v.Brand         AS VehicleBrand,
                                v.Model       AS VehicleModel,

                                -- Zone (relación)
                                z.Id            AS ZoneId,
                                z.Name        AS ZoneName,
                                z.Description AS ZoneDescription,
                                z.DeliveryDays  AS ZoneDeliveryDays,
                                z.RequestDays   AS ZoneRequestDays,
                                z.ImageUrl      AS ZoneImageUrl
                            FROM Distributions d
                            JOIN Users u                ON u.Id = d.UserId
                            JOIN Roles r                ON r.Id = u.RoleId
                            LEFT JOIN Vehicles v        ON v.Id = d.VehicleId
                            LEFT JOIN Distributions_Zones dz ON dz.DistributionId = d.Id
                            LEFT JOIN Zones z           ON z.Id = dz.ZoneId
                            ";



        public async Task<List<Distribution>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Observations", "Date", "UserId", "VehicleId", "DistributionId" };

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    var dict = new Dictionary<int, Distribution>();
                    var seenZone = new Dictionary<int, HashSet<int>>();

                    while (reader.Read())
                    {
                        int distId = reader.GetInt32(reader.GetOrdinal("DistributionId"));

                        if (!dict.TryGetValue(distId, out var dist))
                        {
                            dist = DistributionMapper.FromReader(reader);
                            dist.Zones = new List<Zone>();
                            dict.Add(distId, dist);
                            seenZone[distId] = new HashSet<int>();
                        }

                        int ordZoneId = reader.GetOrdinal("ZoneId");
                        if (!reader.IsDBNull(ordZoneId))
                        {
                            int zoneId = reader.GetInt32(ordZoneId);
                            if (seenZone[distId].Add(zoneId))
                                dict[distId].Zones.Add(ZoneMapper.FromReader(reader, "Zone"));
                        }
                    }

                    return dict.Values.ToList();
                },
                options: options,
                allowedFilterColumns: allowedFilters,
                tableAlias: "d"
            );
        }

        public async Task<Distribution?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: baseQuery + "WHERE d.Id = @Id",
                map: reader =>
                {
                    Distribution? dist = null;
                    var seenZoneIds = new HashSet<int>();

                    while (reader.Read())
                    {
                        if (dist == null)
                        {
                            dist = DistributionMapper.FromReader(reader);
                            dist.Zones = new List<Zone>();
                        }

                        int ordZoneId = reader.GetOrdinal("ZoneId");
                        if (!reader.IsDBNull(ordZoneId))
                        {
                            int zoneId = reader.GetInt32(ordZoneId);
                            if (seenZoneIds.Add(zoneId))
                                dist.Zones.Add(ZoneMapper.FromReader(reader, "Zone"));
                        }
                    }

                    return dist;
                },
                options: new QueryOptions(),
                tableAlias: "d",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }




        private async Task DeleteDistributionZonesAsync(int distributionId)
        {
            await ExecuteWriteAsync(
                "DELETE FROM Distributions_Zones WHERE DistributionId = @DistributionId",
                cmd => cmd.Parameters.AddWithValue("@DistributionId", distributionId)
            );
        }

        private async Task AddDistributionZonesAsync(int distributionId, List<Zone> zones)
        {
            if (zones?.Any() != true) return;

            await ExecuteWriteAsync(
                QueryBuilder.BuildBulkInsertQuery("Distributions_Zones", zones.Count, "DistributionId", "ZoneId"),
                cmd =>
                {
                    for (int i = 0; i < zones.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@DistributionId{i}", distributionId);
                        cmd.Parameters.AddWithValue($"@ZoneId{i}", zones[i].Id);
                    }
                }
            );
        }

        private async Task DeleteDistributionDeliveriesAsync(int distributionId)
        {
            await ExecuteWriteAsync(
                "DELETE FROM Distributions_Deliveries WHERE DistributionId = @DistributionId",
                cmd => cmd.Parameters.AddWithValue("@DistributionId", distributionId)
            );
        }

        private async Task AddDistributionDeliveriesAsync(int distributionId, List<DistributionDelivery> deliveries)
        {
            if (deliveries?.Any() != true) return;

            await ExecuteWriteAsync(
                QueryBuilder.BuildBulkInsertQuery(
                    "Distributions_Deliveries",
                    deliveries.Count,
                    "DistributionId", "DeliveryId", "Position"
                    
                ),
                cmd =>
                {
                    for (int i = 0; i < deliveries.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@DistributionId{i}", distributionId);
                        cmd.Parameters.AddWithValue($"@DeliveryId{i}", deliveries[i].Delivery.Id);
                        cmd.Parameters.AddWithValue($"@Position{i}", deliveries[i].Position);
                    }
                }
            );
        }
    }
}
