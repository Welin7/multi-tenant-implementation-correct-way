using Microsoft.EntityFrameworkCore;

namespace MultiTenantExample2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }

        private readonly TenantService _tenantService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = (TenantService?)tenantService ?? throw new ArgumentNullException(nameof(tenantService));
            TenantId = _tenantService.GetCurrentTenant()?.TId ?? string.Empty;
            Products = Set<Product>();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == TenantId);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenatnConnectionString = _tenantService.GetConnectionString();

            if (!string.IsNullOrEmpty(tenatnConnectionString))
            {
                var dbProvider = _tenantService.GetDataBaseProvider();

                if (dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenatnConnectionString);
                }
            }
            else
            {
                throw new Exception("No Connection String Found");
            }
        }

        public override Task <int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity is IMustHaveTenant entity)
                {
                    entity.TenantId = TenantId;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
} 
