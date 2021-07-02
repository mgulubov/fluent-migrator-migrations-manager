namespace MigrationsManager.Tests.Integration.EF.Models
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
