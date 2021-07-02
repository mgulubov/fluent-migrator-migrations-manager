namespace MigrationsManager.Tests.Integration.Migrations
{
    using FluentMigrator.Runner.VersionTableInfo;

    [VersionTableMetaData]
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
}
