using System.ComponentModel.DataAnnotations;

namespace WhereIsMyGrade.Models
{
    public class course_has_students
    {
        [Key]
        public int COURSE_idCOURSE { get; set; }

        [Required]
        public int STUDENTS_RegistrationNumber { get; set; }

        [Required]
        public int GradeCourseStudent { get; set; }
    }
}
