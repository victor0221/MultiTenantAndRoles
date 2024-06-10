using System.ComponentModel.DataAnnotations;

namespace MultiTenantAndRolesTest.Models
{
    public class Permission
    {
        public int Id { get; set; }

        private string name;

        [Required(ErrorMessage = "Name is required")]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NormalisedName = NormalizeName(name);
            }
        }

        public string NormalisedName { get; private set; }

        private string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            name = name.ToLower();
            return char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}
