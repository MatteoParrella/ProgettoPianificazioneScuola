using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Models;
using StudyPlannerPro.Services;

namespace StudyPlannerPro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExamsController(AppDbContext context)
    {
        _context = context;
    }

    // 1. Ottieni tutti gli esami
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
    {
        return await _context.Exams.ToListAsync();
    }

    // 2. Aggiungi un nuovo esame
    [HttpPost]
    public async Task<ActionResult<Exam>> PostExam(Exam exam)
    {
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetExams), new { id = exam.Id }, exam);
    }

    // 3. Livello di Stress (Basato sulle ore effettive/giorno)
    [HttpGet("{id}/stress-level")]
    public async Task<ActionResult<object>> GetStressLevel(int id, [FromServices] PlannerService planner)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        int days = planner.GetDaysRemaining(exam.ExamDate);
        double hoursPerDay = exam.EstimatedTotalHours / (days > 0 ? days : 1);

        return new {
            ExamTitle = exam.Title,
            Status = hoursPerDay > 4 ? "Rosso (Urgente)" : hoursPerDay > 2 ? "Giallo (Attenzione)" : "Verde (Tranquillo)"
        };
    }

    // 4. Piano Giornaliero Adattivo (Usa il metodo del Service)
    [HttpGet("{id}/piano-giornaliero")]
    public async Task<ActionResult<object>> GetDailyPlan(int id, [FromServices] PlannerService planner)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        // Chiamiamo il metodo del Service che abbiamo sistemato prima
        var plan = planner.GenerateDailyPlan(exam.EstimatedTotalHours, exam.CompletedHours, exam.ExamDate);

        return new {
            Esame = exam.Title,
            Suggerimento = plan
        };
    }

    // 5. Elimina Esame
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExam(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        _context.Exams.Remove(exam);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 6. Riepilogo Globale (Somma di tutti gli esami)
    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetGlobalSummary([FromServices] PlannerService planner)
    {
        var exams = await _context.Exams.ToListAsync();
        double totalHoursToday = 0;

        foreach (var exam in exams)
        {
            int daysLeft = planner.GetDaysRemaining(exam.ExamDate);
            if (daysLeft > 0)
            {
                // Sommiamo il carico rimanente diviso i giorni rimasti
                double remaining = exam.EstimatedTotalHours - exam.CompletedHours;
                if (remaining > 0)
                {
                    totalHoursToday += remaining / daysLeft;
                }
            }
        }

        return new {
            TotalHours = Math.Round(totalHoursToday, 1),
            Message = totalHoursToday > 6 ? "Giornata intensa! 😰" : "Carico gestibile. 👍"
        };
    }

    // 7. Aggiungi ore studiate
    [HttpPost("{id}/add-hours")]
    public async Task<IActionResult> AddStudyHours(int id, [FromBody] double hours)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        exam.CompletedHours += hours;
        await _context.SaveChangesAsync();
        return Ok(exam.CompletedHours);
    }
    [HttpPost("{id}/reset-hours")]
    public async Task<IActionResult> ResetStudyHours(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        exam.CompletedHours = 0;
        await _context.SaveChangesAsync();
        return Ok();
    }
}