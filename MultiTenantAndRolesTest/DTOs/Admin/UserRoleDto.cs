using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.DTOs.Admin
{
    public class UserRoleDto
    {
        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }
    }
}
