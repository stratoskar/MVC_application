using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhereIsMyGrade.Models
{
    public class course
    {
        [Key]
        public int IdCourse { get; set; }

        [Required]
        [MaxLength(60)]
        public string CourseTitle { get; set; }

        [Required]
        [MaxLength(25)]
        public string CourseSemester { get; set; }

        [ForeignKey("professors")]
        public int PROFESSORS_AFM { get; set; }

        // This is the Foreign key
        public virtual professors professors { get; set; }
    }
}
