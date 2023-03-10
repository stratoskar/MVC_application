using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhereIsMyGrade.Models
{
    public class secretaries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Phonenumber { get; set; }

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
        [ForeignKey("users")]
        public string USERS_username { get; set; }

        // This is the Foreign key
        public virtual users users { get; set; }
    }
}
