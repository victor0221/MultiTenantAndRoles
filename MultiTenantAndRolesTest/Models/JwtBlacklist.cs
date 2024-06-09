namespace MultiTenantAndRolesTest.Models
{
    public class JwtBlacklist
    {
        public int Id { get; set; }
        public string token { get; set; }
        public DateTime expires { get; set; }
    }
}
