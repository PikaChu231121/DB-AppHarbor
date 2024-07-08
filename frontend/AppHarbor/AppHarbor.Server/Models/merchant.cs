using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppHarbor.Server.Models
{
    [Table("merchant", Schema = "C##APPHARBOR")]

    public class merchant
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
