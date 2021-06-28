namespace FluentMigrator.MigrationsManager.IntegrationTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MigrationsManagerTestCase
    {
        private string connectionString;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.connectionString = this.TestContext.Properties["connectionString"].ToString();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.connectionString = string.Empty;
        }
    }
}
