using System.ComponentModel.DataAnnotations;

namespace WhereIsMyGrade.Models
{
    public class professors
    {
        [Key]
        public int AFM { get; set; }

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [Required]
        [MaxLength(45)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(45)]
        public string Department { get; set; }

        [Required]
        [MaxLength(45)]
        public string USERS_username { get; set; }
    }
}
