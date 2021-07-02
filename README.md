# MigrationsManager

Migrations Manager is a simple abstraction, built on top of FluentMigrator. It exposes a simple interface for moving the database to a specific version.


## Example Usage
```
namespace MigrationsManagerTest
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    using FluentMigrator;
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.VersionTableInfo;

    using MigrationsManager;
    using MigrationsManager.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=FluentMigratorTestDb;Trusted_Connection=True;";
            IServiceProvider serviceProvider = BuildServiceProvider(connectionString);

            IMigrationsManager migrationsManager = new MigrationsManager(serviceProvider);

            migrationsManager.MigrateTo(1);
        }

        private static IServiceProvider BuildServiceProvider(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb.AddSqlServer().WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(VersionInfo).Assembly).For.Migrations()
                )
                .AddScoped<IVersionTableMetaData, VersionInfo>()
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }

    public class VersionInfo : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }

        public string TableName => "VersionInfo";

        public string ColumnName => "Version";

        public string AppliedOnColumnName => "AppliedOn";

        public string DescriptionColumnName => "Description";

        public string SchemaName => string.Empty;

        public string UniqueIndexName => "UcVersion";

        public bool OwnsSchema => true;
    }

    [Migration(1, "Create a test Users table")]
    public class CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Username").AsString(50).Unique().NotNullable()
                .WithColumn("FirstName").AsString(300).Nullable()
                .WithColumn("LastName").AsString(300).Nullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
```
