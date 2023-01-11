using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhereIsMyGrade.Models
{
    public class course_has_students
    {
        [ForeignKey("course")]
        public int COURSE_idCOURSE { get; set; }

        [ForeignKey("students")]
        public int STUDENTS_RegistrationNumber { get; set; }

        [Required]
        public int GradeCourseStudent { get; set; }

        // This are the Foreign keys
        public virtual course course { get; set; }
        public virtual students students { get; set; }
    }
}
