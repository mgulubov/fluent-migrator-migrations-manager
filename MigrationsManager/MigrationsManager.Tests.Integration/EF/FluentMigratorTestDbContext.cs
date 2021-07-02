namespace MigrationsManager.Tests.Integration.EF
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class FluentMigratorTestDbContext : DbContext
    {
        public FluentMigratorTestDbContext(DbContextOptions<FluentMigratorTestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<VersionInfo> Versions { get; set; }
    }
}
