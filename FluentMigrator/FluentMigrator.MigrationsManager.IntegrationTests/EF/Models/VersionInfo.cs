namespace FluentMigrator.MigrationsManager.IntegrationTests.EF.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 
    [Table("VersionInfo")]
    public class VersionInfo
    {
        [Column("Version")]
        [Key]
        public long Version { get; set; }
    }
}
