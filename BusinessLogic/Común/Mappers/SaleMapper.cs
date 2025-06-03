using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSale;
using BusinessLogic.DTOs.DTOsSaleItem;

namespace BusinessLogic.Común.Mappers
{
    public static class SaleMapper
    {
        public static Sale toDomain(AddSaleRequest addSaleRequest)
        {
            return new Sale(0, addSaleRequest.Date, addSaleRequest.Status, new List<SaleItem>());
        }
        public static Sale.UpdatableData toDomain(UpdateSaleRequest updateSaleRequest)
        {
            return new Sale.UpdatableData
            {
                Date = updateSaleRequest.Date,
                Status = updateSaleRequest.Status
            };
        }
        public static SaleResponse toResponse(Sale sale)
        {
            return new SaleResponse
            {
                Id = sale.Id,
                Date = sale.Date,
                Status = sale.Status,
                SaleItems = sale.SaleItems.Select(si => new SaleItemResponse
                {
                    Id = si.Id,
                    Quantity = si.Quantity,
                    UnitPrice = si.UnitPrice,
                    Product = null
                }).ToList()
            };

        }
    }
}
