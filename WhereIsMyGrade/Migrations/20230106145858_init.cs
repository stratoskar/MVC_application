using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhereIsMyGrade.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    IdCourse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseTitle = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CourseSemester = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PROFESSORS_AFM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.IdCourse);
                });

            migrationBuilder.CreateTable(
                name: "course_has_students",
                columns: table => new
                {
                    COURSE_idCOURSE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STUDENTS_RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    GradeCourseStudent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_has_students", x => x.COURSE_idCOURSE);
                });

            migrationBuilder.CreateTable(
                name: "professors",
                columns: table => new
                {
                    AFM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    USERS_username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professors", x => x.AFM);
                });

            migrationBuilder.CreateTable(
                name: "secretaries",
                columns: table => new
                {
                    Phonenumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    USERS_username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_secretaries", x => x.Phonenumber);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    USERS_username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "course_has_students");

            migrationBuilder.DropTable(
                name: "professors");

            migrationBuilder.DropTable(
                name: "secretaries");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
