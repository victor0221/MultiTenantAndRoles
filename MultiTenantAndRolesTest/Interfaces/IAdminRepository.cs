using MultiTenantAndRolesTest.DTOs.Admin;
using MultiTenantAndRolesTest.Models;

namespace MultiTenantAndRolesTest.Interfaces
{
    public interface IAdminRepository
    {
        Task<Role> CreateRoleAsync(Role role);
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<RolePermission> CreateRolePermissionAsync(RolePermissionDto rolePermission);
        Task<UserRole> CreateUserRoleAsync(UserRoleDto userRole);
    }
}
