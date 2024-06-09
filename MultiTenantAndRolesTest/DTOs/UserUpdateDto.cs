using MultiTenantAndRolesTest.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.DTOs
{
    public class UserUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
