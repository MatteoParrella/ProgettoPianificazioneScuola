using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Models;

namespace StudyPlannerPro.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Assicurati che ci siano ENTRAMBE queste righe
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Subject> Subjects { get; set; } // <--- Questa mancava!
}