namespace MigrationsManager.Tests.Integration.Migrations
{
    using FluentMigrator;

    [Migration(2, "Add Purchases table along with relation to Users")]
    public class AddPurchasesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Purchase")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Amount").AsDecimal().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("fk_purchase_id_user_id", "User", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_purchase_id_user_id").OnTable("Purchase");
            Delete.Table("Purchase");
        }
    }
}
