using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ClientMapper
    {
        public static Client FromReader(SqlDataReader reader)
        {
            var bankName = reader.GetString(reader.GetOrdinal("Bank")).Trim();
            var accountName = reader.GetString(reader.GetOrdinal("AccountName")).Trim();
            var accountNumber = reader.GetString(reader.GetOrdinal("AccountNumber")).Trim();

            Enum.TryParse(bankName, ignoreCase: true, out Bank bankEnum);

            var bankAccount = new BankAccount(
                id: 0,
                accountName: accountName,
                accountNumber: accountNumber,
                bank: bankEnum
            );

            var zone = new Zone(
                id: reader.GetInt32(reader.GetOrdinal("ZoneId"))
            );

            zone.Name = reader.GetString(reader.GetOrdinal("ZoneName"));
            zone.Description = reader.GetString(reader.GetOrdinal("ZoneDescription"));

            if (!reader.IsDBNull(reader.GetOrdinal("ZoneBlobName")))
            {
                var blobName = reader.GetString(reader.GetOrdinal("ZoneBlobName"));
                zone.ImageUrl = $"https://chriska.blob.core.windows.net/images/{blobName}";
            }

            var client = new Client(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                rut: reader.GetString(reader.GetOrdinal("RUT")),
                razonSocial: reader.GetString(reader.GetOrdinal("RazonSocial")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                mapsAddress: reader.GetString(reader.GetOrdinal("MapsAddress")),
                schedule: reader.GetString(reader.GetOrdinal("Schedule")),
                phone: reader.GetString(reader.GetOrdinal("Phone")),
                contactName: reader.GetString(reader.GetOrdinal("ContactName")),
                email: reader.GetString(reader.GetOrdinal("Email")),
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                bankAccounts: new List<BankAccount> { bankAccount },
                loanedCrates: reader.GetInt32(reader.GetOrdinal("LoanedCrates")),
                qualification: reader.GetString(reader.GetOrdinal("Qualification")),
                zone: zone
            );

            return client;
        }
    }
}
