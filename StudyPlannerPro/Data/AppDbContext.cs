using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Models; // Assicurati che il namespace sia corretto

namespace StudyPlannerPro.Data
{
    // L'errore CS0311 dice che manca questo ": DbContext"
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Exam> Exams { get; set; }
    }
}