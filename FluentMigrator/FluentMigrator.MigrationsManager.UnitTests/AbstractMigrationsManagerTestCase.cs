namespace FluentMigrator.MigrationsManager.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Extensions.DependencyInjection;

    using FluentMigrator.Runner;
    using Moq;

    using FluentMigrator.MigrationsManager.Interfaces;

    [TestClass]
    public abstract class AbstractMigrationsManagerTestCase
    {
        protected const long TargetVersion = 5;

        protected abstract IMigrationsManager GetMigrationsManager(IServiceProvider serviceProvider);

        public void TestMigrationsManagerIsNotNull()
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            IMigrationsManager migrationsManager = this.GetMigrationsManager(serviceProviderMock.Object);

            Assert.IsNotNull(migrationsManager);
        }

        [TestMethod]
        public void TestMigrationsManagerMigrateToReturnsTrueWhenHasHigherMigrationsAndNoLowerMigrations()
        {
            Mock<IMigrationRunner> migrationRunnerMock = new Mock<IMigrationRunner>();
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyUp(TargetVersion)).Returns(true);
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyDown(TargetVersion)).Returns(false);

            IServiceProvider serviceProvider = this.BuildServiceProvider(migrationRunnerMock.Object);
            IMigrationsManager migrationsManager = this.GetMigrationsManager(serviceProvider);

            bool didMigrate = migrationsManager.MigrateTo(TargetVersion);

            Assert.IsTrue(didMigrate);
        }

        [TestMethod]
        public void TestMigrationsManagerMigrateToReturnsFalseWhenHasNoHigherMigrationsAndNoLowerMigrations()
        {
            Mock<IMigrationRunner> migrationRunnerMock = new Mock<IMigrationRunner>();
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyUp(TargetVersion)).Returns(false);
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyDown(TargetVersion)).Returns(false);

            IServiceProvider serviceProvider = this.BuildServiceProvider(migrationRunnerMock.Object);
            IMigrationsManager migrationsManager = this.GetMigrationsManager(serviceProvider);

            bool didMigration = migrationsManager.MigrateTo(TargetVersion);

            Assert.IsFalse(didMigration);
        }

        [TestMethod]
        public void TestMigrationsManagerMigrateToReturnsTrueWhenHasNoHigherMigrationsAndHasLowerMigrations()
        {
            Mock<IMigrationRunner> migrationRunnerMock = new Mock<IMigrationRunner>();
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyUp(TargetVersion)).Returns(false);
            migrationRunnerMock.Setup(mr => mr.HasMigrationsToApplyDown(TargetVersion)).Returns(true);

            IServiceProvider serviceProvider = this.BuildServiceProvider(migrationRunnerMock.Object);
            IMigrationsManager migrationsManager = this.GetMigrationsManager(serviceProvider);

            bool didMigration = migrationsManager.MigrateTo(TargetVersion);

            Assert.IsTrue(didMigration);
        }

        private IServiceProvider BuildServiceProvider(IMigrationRunner migrationsRunner)
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();
            Mock<IServiceProvider> scopeServiceProviderMock = new Mock<IServiceProvider>();
            Mock<IServiceScopeFactory> serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            scopeServiceProviderMock.Setup(ssp => ssp.GetService(typeof(IMigrationRunner))).Returns(migrationsRunner);
            serviceScopeMock.Setup(sc => sc.ServiceProvider).Returns(scopeServiceProviderMock.Object);
            serviceScopeFactoryMock.Setup(ssf => ssf.CreateScope()).Returns(serviceScopeMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

            return serviceProviderMock.Object;
        }
    }
}
