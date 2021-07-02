namespace MigrationsManager.Tests.Integration.EF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public class User
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Username")]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Column("FirstName")]
        [MaxLength(300)]
        public string FirstName { get; set; }

        [Column("LastName")]
        [MaxLength(300)]
        public string LastName { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
