using MultiTenantAndRolesTest.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "User token are requried")]
        public string Password { get; set; }
        public List<Role> Roles { get; set; }
    }
}
