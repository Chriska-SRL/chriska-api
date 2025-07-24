using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.Común.Mappers;
using System.Xml.Linq;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class RolesSubSystem
    {
        private readonly IRoleRepository _roleRepository;

        public RolesSubSystem(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponse> AddRoleAsync(AddRoleRequest request)
        {
            var newRole = RoleMapper.ToDomain(request);
            newRole.Validate();

            var existing = await _roleRepository.GetByNameAsync(newRole.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.", nameof(newRole.Name));

            var added = await _roleRepository.AddAsync(newRole);
            return RoleMapper.ToResponse(added);
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var existingRole = await _roleRepository.GetByIdAsync(request.Id)
                                 ?? throw new ArgumentException("No se encontró el rol seleccionado.", nameof(request.Id));

            var existing = await _roleRepository.GetByNameAsync(request.Name);
            if (existingRole.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.", nameof(request.Name));

            var updatedData = RoleMapper.ToUpdatableData(request);
            existingRole.Update(updatedData);

            var updated = await _roleRepository.UpdateAsync(existingRole);
            return RoleMapper.ToResponse(updated);
        }
        public async Task<RoleResponse> DeleteRoleAsync(DeleteRoleRequest request)
        {
            var deleted = await _roleRepository.GetByIdWithUsersAsync(request.Id)
                          ?? throw new ArgumentException("No se encontró el rol seleccionado.", nameof(request.Id));

            if (deleted.Users.Any())
                throw new InvalidOperationException("No se puede eliminar el rol porque tiene usuarios asociados.");

            await _roleRepository.DeleteAsync(deleted);

            return RoleMapper.ToResponse(deleted);
        }

        public async Task<RoleResponse> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id)
                       ?? throw new ArgumentException("No se encontró el rol seleccionado.", nameof(id));

            return RoleMapper.ToResponse(role);
        }

        public async Task<List<RoleResponse>> GetAllRolesAsync(QueryOptions options)
        {
            var roles = await _roleRepository.GetAllAsync(options);
            return roles.Select(RoleMapper.ToResponse).ToList();
        }
    }
}
