namespace FluentMigrator.MigrationsManager.IntegrationTests
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using FluentMigrator.Runner;
    using FluentMigrator.Runner.VersionTableInfo;

    using EF;
    using EF.Models;
    using Migrations;

    using FluentMigrator.MigrationsManager;
    using FluentMigrator.MigrationsManager.Interfaces;

    [TestClass]
    public class MigrationsManagerTestCase
    {
        protected FluentMigratorTestDbContext dbContext;
        protected IMigrationsManager migrationsManager;

        private string connectionString;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.connectionString = this.TestContext.Properties["connectionString"].ToString();

            DbContextOptionsBuilder<FluentMigratorTestDbContext> builder = new DbContextOptionsBuilder<FluentMigratorTestDbContext>();
            builder.UseSqlServer(connectionString);
            DbContextOptions<FluentMigratorTestDbContext> dbContextOptions = builder.Options;

            this.dbContext = new FluentMigratorTestDbContext(dbContextOptions);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.connectionString = string.Empty;
            this.dbContext.Dispose();
        }

        [TestMethod]
        public void TestApplyAllMigrationsUpwardsFromZero()
        {
            long expectedVersion = 3;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo actualVersion = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(actualVersion);
            Assert.AreEqual(expectedVersion, actualVersion.Version);
        }

        [TestMethod]
        public void TestApplyAllMigrationsDownwardsToZero()
        {
            long maxVersion = 3;
            long expectedVersion = 0;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(maxVersion);

            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo actualVersion = this.dbContext.Versions.FirstOrDefault();

            Assert.IsNull(actualVersion);
        }

        [TestMethod]
        public void TestApplyAllMigrationsUpwardsFromOne()
        {
            long expectedVersion = 3;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(1);

            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(versionInfo);
            Assert.AreEqual(expectedVersion, versionInfo.Version);
        }

        [TestMethod]
        public void TestApplyOneMigrationFromZero()
        {
            long expectedVersion = 1;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());

            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(versionInfo);
            Assert.AreEqual(expectedVersion, versionInfo.Version);
        }

        [TestMethod]
        public void TestApplyOneMigrationFromOne()
        {
            long expectedVersion = 2;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(1);

            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(versionInfo);
            Assert.AreEqual(expectedVersion, versionInfo.Version);
        }

        [TestMethod]
        public void TestMigrateToZeroFromOne()
        {
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(3);

            this.migrationsManager.MigrateTo(0);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNull(versionInfo);
        }

        [TestMethod]
        public void TestMigrateToOneFromMax()
        {
            long expectedVersion = 1;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(3);

            this.migrationsManager.MigrateTo(expectedVersion);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(versionInfo);
            Assert.AreEqual(expectedVersion, versionInfo.Version);
        }

        [TestMethod]
        public void TestMigrateUpToNonExistingVersionFromZero()
        {
            long expectedVersion = 3;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(expectedVersion * 2);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNotNull(versionInfo);
            Assert.AreEqual(expectedVersion, versionInfo.Version);
        }

        [TestMethod]
        public void TestMigrateDownToNonExistingVersionFromMax()
        {
            long max = 3;
            this.migrationsManager = new MigrationsManager(this.BuildServiceProvider());
            this.migrationsManager.MigrateTo(max);

            this.migrationsManager.MigrateTo(-1);

            EF.Models.VersionInfo versionInfo = this.dbContext.Versions.OrderByDescending(v => v.Version).FirstOrDefault();

            Assert.IsNull(versionInfo);
        }

        private IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb.AddSqlServer().WithGlobalConnectionString(this.connectionString)
                        .ScanIn(typeof(Migrations.VersionInfo).Assembly).For.Migrations()
                )
                .AddScoped<IVersionTableMetaData, Migrations.VersionInfo>()
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
