using MultiTenantAndRolesTest.Models;

namespace MultiTenantAndRolesTest.Interfaces
{
    public interface ITokenRepository
    {
        Task<string> CreateToken(User user);
    }
}
