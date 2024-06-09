using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        [Required(ErrorMessage = "Permission Id is required")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
