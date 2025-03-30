namespace MultiTenantExample2.Services
{
    public interface ITenantService
    {
        public string? GetDataBaseProvider();
        public string? GetConnectionString();
        public Tenant? GetCurrentTenant();
    }
}
