using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppHarbor.Server.Models
{
    [Table("user", Schema = "C##APPHARBOR")]

    public class UserLoginModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
    public class user
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nickname { get; set; }

        [MaxLength(255)]
        public string Avatar { get; set; }

        [Required]
        public DateTime RegisterTime { get; set; }

        public decimal Credit { get; set; }

        [Required]
        [MaxLength(255)]
        public string State { get; set; }

    }
}
