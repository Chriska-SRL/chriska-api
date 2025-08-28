using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Common;
using BusinessLogic.DTOs;

namespace BusinessLogic.SubSystem
{
    public class RolesSubSystem
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RolesSubSystem(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<RoleResponse> AddRoleAsync(AddRoleRequest request)
        {
            var newRole = RoleMapper.ToDomain(request);
            newRole.Validate();

            Role existing = await _roleRepository.GetByNameAsync(newRole.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.");

            var added = await _roleRepository.AddAsync(newRole);
            return RoleMapper.ToResponse(added);
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var existingRole = await _roleRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el rol seleccionado.");

            var existing = await _roleRepository.GetByNameAsync(request.Name);
            if (existingRole.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.");

            var updatedData = RoleMapper.ToUpdatableData(request);
            existingRole.Update(updatedData);

            var updated = await _roleRepository.UpdateAsync(existingRole);
            return RoleMapper.ToResponse(updated);
        }

        public async Task<RoleResponse> DeleteRoleAsync(DeleteRequest request)
        {
            Role role = await _roleRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el rol seleccionado.");

            var options = new QueryOptions
            {
                Filters = new Dictionary<string, string>
                {
                    { "RoleId", request.Id.ToString() }
                }
            };

            var users = await _userRepository.GetAllAsync(options);

            if (users.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el rol porque tiene usuarios asociados.");
            }

            role.MarkAsDeleted(request.getUserId(),request.AuditLocation);
            await _roleRepository.DeleteAsync(role);
            return RoleMapper.ToResponse(role);
        }

        public async Task<RoleResponse> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el rol seleccionado.");

            return RoleMapper.ToResponse(role);
        }

        public async Task<List<RoleResponse>> GetAllRolesAsync(QueryOptions options)
        {
            var roles = await _roleRepository.GetAllAsync(options);
            return roles.Select(RoleMapper.ToResponse).ToList();
        }
    }
}
