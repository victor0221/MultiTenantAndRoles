using MultiTenantAndRolesTest.DTOs;
using MultiTenantAndRolesTest.Models;

namespace MultiTenantAndRolesTest.Interfaces
{
    public interface IUserManager
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hash);
        Task<string> CheckPasswordPolicy(string password);
        Task<List<Role>> GetRolesAsync(User user);
        Task<UserRegisterLoginSuccessDto> CreateUserAsync(UserRegisterDto user);
        Task<User> GetOneUserByEmailAsync(string userEmail);
    }
}
