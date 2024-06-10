using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
