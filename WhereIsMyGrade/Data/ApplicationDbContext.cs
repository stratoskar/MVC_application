using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WhereIsMyGrade.Models;

namespace WhereIsMyGrade.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // create the tables of the database
        public DbSet<users> user { get; set; }
        public DbSet<course> course { get; set; }
        public DbSet<course_has_students> course_has_students { get; set; }
        public DbSet<professors> professors { get; set; }
        public DbSet<secretaries> secretaries { get; set; }
        public DbSet<students> students { get; set; }
    }
}
