using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MultiTenantAndRolesTest.Data;
using MultiTenantAndRolesTest.DTOs.Admin;
using MultiTenantAndRolesTest.Interfaces;
using MultiTenantAndRolesTest.Models;

namespace MultiTenantAndRolesTest.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserManager _userManager;
        public AdminRepository(AppDbContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            try
            {
                await _context.Permission.AddAsync(permission);
                await _context.SaveChangesAsync();

                return permission;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && (innerException.Number == 2601 || innerException.Number == 2627))
                {
                    throw new Exception("Permission with that name already exists");
                }
                else
                {
                    throw new Exception("An error occurred while saving the entity changes. See the inner exception for details.");
                }
            }
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            try
            {
                await _context.Role.AddAsync(role);
                await _context.SaveChangesAsync();

                return role;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && (innerException.Number == 2601 || innerException.Number == 2627))
                {
                    throw new Exception("Role with that name already exists");
                }
                else
                {
                    throw new Exception("An error occurred while saving the entity changes. See the inner exception for details.");
                }
            }
        }

        public async Task<RolePermission> CreateRolePermissionAsync(RolePermissionDto rolePermissionDto)
        {
            try
            {
                var role = await _context.Role.FindAsync(rolePermissionDto.RoleId);
                if (role == null)
                {
                    throw new ArgumentException($"Role with Id {rolePermissionDto.RoleId} does not exist");
                }

                var permission = await _context.Permission.FindAsync(rolePermissionDto.PermissionId);
                if (permission == null)
                {
                    throw new ArgumentException($"Permission with Id {rolePermissionDto.PermissionId} does not exist");
                }

                var RolePerm = new RolePermission
                {
                    PermissionId = rolePermissionDto.PermissionId,
                    Permission = permission,
                    RoleId = rolePermissionDto.RoleId,
                    Role = role   
                };

                await _context.RolePermission.AddAsync(RolePerm);
                await _context.SaveChangesAsync();

                return RolePerm;
            }
            catch(DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the entity changes. See the inner exception for details.");
            }
        }

        public async Task<UserRole> CreateUserRoleAsync(UserRoleDto userRoledDto)
        {
            try
            {
                var role = await _context.Role.FindAsync(userRoledDto.RoleId);
                if (role == null)
                {
                    throw new ArgumentException($"Role with Id {userRoledDto.RoleId} does not exist");
                }

                var user = await _context.User.FindAsync(userRoledDto.UserId);
                if (user == null)
                {
                    throw new ArgumentException($"User with Id {userRoledDto.UserId} does not exist");
                }

                var userRole = new UserRole
                {
                    UserId = userRoledDto.UserId,
                    User = user,
                    RoleId = userRoledDto.RoleId,
                    Role = role,
                };

                await _context.UserRole.AddAsync(userRole);
                await _context.SaveChangesAsync();

                return userRole;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the entity changes. See the inner exception for details.");
            }

        }
    }
}
