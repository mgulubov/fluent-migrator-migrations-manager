namespace FluentMigrator.MigrationsManager.IntegrationTests.Migrations
{
    [Migration(1, "Create a test Users table")]
    public class CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Username").AsString(50, "UTF-8").Unique().NotNullable()
                .WithColumn("FirstName").AsString(300, "UTF-8").Nullable()
                .WithColumn("LastName").AsString(300, "UTF-8").Nullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
