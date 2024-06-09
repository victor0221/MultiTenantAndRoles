using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.DTOs.Admin
{
    public class RolePermissionDto
    {
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Permission Id is required")]
        public int PermissionId { get; set; }
    }
}
