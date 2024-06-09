using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
