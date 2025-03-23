namespace MultiTenantExample2.Settings
{
    public class TenantSettings
    {
        public List<Tenant> Tenants { get; set; } = new();
        public Configuration Defaults { get; set; } = default!;
    }
}
