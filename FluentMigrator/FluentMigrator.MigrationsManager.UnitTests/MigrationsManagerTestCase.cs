namespace FluentMigrator.MigrationsManager.UnitTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using FluentMigrator.MigrationsManager;
    using FluentMigrator.MigrationsManager.Interfaces;

    [TestClass]
    public class MigrationsManagerTestCase : AbstractMigrationsManagerTestCase
    {
        protected override IMigrationsManager GetMigrationsManager(IServiceProvider serviceProvider)
        {
            return new MigrationsManager(serviceProvider);
        }
    }
}
