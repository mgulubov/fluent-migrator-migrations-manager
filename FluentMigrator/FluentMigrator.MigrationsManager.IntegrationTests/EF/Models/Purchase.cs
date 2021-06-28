namespace FluentMigrator.MigrationsManager.IntegrationTests.EF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Purchase")]
    public class Purchase
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Amount")]
        [Required]
        public decimal Amount { get; set; }

        [Column("UserId")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
