using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class SuppliersSubSystem
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersSubSystem(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<SupplierResponse> AddSupplierAsync(AddSupplierRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(UpdateSupplierRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteSupplierAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SupplierResponse>> GetAllSuppliersAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
