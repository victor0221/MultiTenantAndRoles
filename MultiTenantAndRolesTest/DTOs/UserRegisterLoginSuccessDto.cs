using MultiTenantAndRolesTest.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.DTOs
{
    public class UserRegisterLoginSuccessDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "User token are requried")]
        public string Token { get; set; }
        [Required(ErrorMessage = "User Roles are requried")]
        public List<Role> Roles { get; set; }
    }
}
