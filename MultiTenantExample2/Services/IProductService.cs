namespace MultiTenantExample2.Services
{
    public interface IProductService
    {
        Task<Product>CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
