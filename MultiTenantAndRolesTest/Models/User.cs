using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }
    }
}
