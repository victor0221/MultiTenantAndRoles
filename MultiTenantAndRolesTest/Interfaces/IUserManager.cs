using MultiTenantAndRolesTest.DTOs;
using MultiTenantAndRolesTest.Models;

namespace MultiTenantAndRolesTest.Interfaces
{
    public interface IUserManager
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hash);
        Task<string> CheckPasswordPolicy(string password);
        Task<List<Role>> UserRolesGetAsync(User user);
        Task<UserRegisterLoginSuccessDto> UserCreateAsync(UserRegisterDto user);
        Task<User> UserGetByEmailAsync(string userEmail);
        Task<bool> UserDelete(int userId);
        Task<UserUpdateSuccess> UserUpdateAsync(int userId, UserUpdateDto userDto);
        Task<UserUpdateSuccess> UserGetByIdAsync(int userId);
        Task<List<UserUpdateSuccess>> UserGetAllAsync(int page, int pageSize, string? sort, string? order, string? searchTerm, int[]? roles);
    }
}
