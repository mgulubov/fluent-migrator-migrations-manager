namespace MigrationsManager.Tests.Integration.Migrations
{
    using FluentMigrator;

    [Migration(3, "Add Items table along with relation to Purchases table")]
    public class AddItemsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Item")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("PurchaseId").AsInt64().Nullable().ForeignKey("fk_item_purchaseid_purchase_id", "Purchase", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_item_purchaseid_purchase_id").OnTable("Item");
            Delete.Table("Item");
        }
    }
}
