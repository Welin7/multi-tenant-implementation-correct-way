﻿namespace MultiTenantExample2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }

        private readonly ITenantService _tenantService;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetCurrentTenant()?.TId?? string.Empty;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == TenantId);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();

            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantService.GetDataBaseProvider();

                if (dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }

        public override Task <int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = TenantId;                
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
} 
