using System.ComponentModel.DataAnnotations;

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

        [Required]
        public int PROFESSORS_AFM { get; set; }
    }
}
