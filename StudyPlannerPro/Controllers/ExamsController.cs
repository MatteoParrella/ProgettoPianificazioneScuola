using System;

namespace StudyPlannerPro.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Models;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExamsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
    {
        return await _context.Exams.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Exam>> PostExam(Exam exam)
    {
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetExams), new { id = exam.Id }, exam);
    }
    [HttpGet("{id}/stress-level")]
        public async Task<ActionResult<object>> GetStressLevel(int id, [FromServices] StudyPlannerPro.Services.PlannerService planner)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        int days = planner.GetDaysRemaining(exam.ExamDate);
        
        // Calcoliamo lo stress basandoci sulle ore REALI al giorno
        double hoursPerDay = exam.EstimatedTotalHours / (days > 0 ? days : 1);

        return new {
            ExamTitle = exam.Title,
            Status = hoursPerDay > 4 ? "Rosso (Urgente)" : hoursPerDay > 2 ? "Giallo (Attenzione)" : "Verde (Tranquillo)"
        };
    }
    [HttpGet("{id}/piano-giornaliero")]
    public async Task<ActionResult<object>> GetDailyPlan(int id, [FromServices] StudyPlannerPro.Services.PlannerService planner)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        var plan = planner.GenerateDailyPlan(exam.EstimatedTotalHours, exam.ExamDate);

        return new {
            Esame = exam.Title,
            Suggerimento = plan
        };
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExam(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null)
        {
            return NotFound(); // Se l'ID non esiste
        }

        _context.Exams.Remove(exam);
        await _context.SaveChangesAsync();

        return NoContent(); // Risposta 204: Successo senza contenuto
    }
    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetGlobalSummary([FromServices] StudyPlannerPro.Services.PlannerService planner)
    {
        var exams = await _context.Exams.ToListAsync();
        double totalHoursToday = 0;

        foreach (var exam in exams)
        {
            int daysLeft = planner.GetDaysRemaining(exam.ExamDate);
            if (daysLeft > 0)
            {
                // Sommiamo (Ore Totali / Giorni Rimasti) per ogni esame
                totalHoursToday += exam.EstimatedTotalHours / daysLeft;
            }
        }

        return new {
            TotalHours = Math.Round(totalHoursToday, 1),
            Message = totalHoursToday > 6 ? "Giornata intensa! 😰" : "Carico gestibile. 👍"
        };
    }
}
