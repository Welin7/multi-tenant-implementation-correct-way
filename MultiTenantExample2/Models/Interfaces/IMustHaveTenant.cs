namespace MultiTenantExample2.Models.Interfaces
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}
