using Microsoft.Extensions.Options;

namespace MultiTenantAndRolesTest.Repositories
{
    public sealed class TenantProvider
    {
        private const string TenantIdHeaderName = "X-TenantId";

        private readonly IHttpContextAccessor _http;
        private readonly TenantConnectionStrings _connectionStrings;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, IOptions<TenantConnectionStrings> connectionStrings)
        {
            _http = httpContextAccessor;
            _connectionStrings = connectionStrings.Value;
        }

        public int GetTenantId()
        {
            var tenantIdHeader = _http.HttpContext?.Request.Headers[TenantIdHeaderName];

            if(!tenantIdHeader.HasValue || !int.TryParse(tenantIdHeader.Value, out int tenantId))
            {
                throw new InvalidOperationException();
            }
            return tenantId;
        }

        public string GetConnectionString()
        {
            return _connectionStrings.Values[GetTenantId()];
        }

    }
}
