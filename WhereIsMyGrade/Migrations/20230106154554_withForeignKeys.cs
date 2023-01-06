using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhereIsMyGrade.Migrations
{
    public partial class withForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    table.ForeignKey(
                        name: "FK_professors_user_USERS_username",
                        column: x => x.USERS_username,
                        principalTable: "user",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_secretaries_user_USERS_username",
                        column: x => x.USERS_username,
                        principalTable: "user",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_students_user_USERS_username",
                        column: x => x.USERS_username,
                        principalTable: "user",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_course_professors_PROFESSORS_AFM",
                        column: x => x.PROFESSORS_AFM,
                        principalTable: "professors",
                        principalColumn: "AFM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_has_students",
                columns: table => new
                {
                    COURSE_idCOURSE = table.Column<int>(type: "int", nullable: false),
                    STUDENTS_RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    GradeCourseStudent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_has_students", x => new { x.COURSE_idCOURSE, x.STUDENTS_RegistrationNumber });
                    table.ForeignKey(
                        name: "FK_course_has_students_course_COURSE_idCOURSE",
                        column: x => x.COURSE_idCOURSE,
                        principalTable: "course",
                        principalColumn: "IdCourse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_has_students_students_STUDENTS_RegistrationNumber",
                        column: x => x.STUDENTS_RegistrationNumber,
                        principalTable: "students",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_PROFESSORS_AFM",
                table: "course",
                column: "PROFESSORS_AFM");

            migrationBuilder.CreateIndex(
                name: "IX_course_has_students_STUDENTS_RegistrationNumber",
                table: "course_has_students",
                column: "STUDENTS_RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_professors_USERS_username",
                table: "professors",
                column: "USERS_username");

            migrationBuilder.CreateIndex(
                name: "IX_secretaries_USERS_username",
                table: "secretaries",
                column: "USERS_username");

            migrationBuilder.CreateIndex(
                name: "IX_students_USERS_username",
                table: "students",
                column: "USERS_username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_has_students");

            migrationBuilder.DropTable(
                name: "secretaries");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "professors");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
