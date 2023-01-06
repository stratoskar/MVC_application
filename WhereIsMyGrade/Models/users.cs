using System.ComponentModel.DataAnnotations;

namespace WhereIsMyGrade.Models
{
    public class users
    {
        [Key]
        [MaxLength(45)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(45)]
        public string Role { get; set; }
    }
}
