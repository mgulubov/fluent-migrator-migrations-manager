namespace MigrationsManager.Tests.Unit
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Interfaces;

    [TestClass]
    public class MigrationsManagerTestCase : AbstractMigrationsManagerTestCase
    {
        protected override IMigrationsManager GetMigrationsManager(IServiceProvider serviceProvider)
        {
            return new MigrationsManager(serviceProvider);
        }
    }
}
