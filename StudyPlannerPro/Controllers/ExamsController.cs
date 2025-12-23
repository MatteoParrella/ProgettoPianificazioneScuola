using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Models;
using StudyPlannerPro.Services;

namespace StudyPlannerPro.Controllers;

[Authorize] // <--- Solo gli utenti loggati possono accedere a questi endpoint
[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExamsController(AppDbContext context)
    {
        _context = context;
    }

    // Helper per recuperare l'ID dell'utente loggato in modo veloce
    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

    // 1. Ottieni solo gli esami dell'utente loggato
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
    {
        var userId = GetUserId();
        return await _context.Exams
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }

    // 2. Aggiungi un nuovo esame collegandolo all'utente
    [HttpPost]
    public async Task<ActionResult<Exam>> PostExam(Exam exam)
    {
        // 1. Controllo data (usando il nome corretto ExamDate)
        if (exam.ExamDate.Date < DateTime.Today)
        {
            return BadRequest("La data dell'esame non può essere nel passato.");
        }

        // 2. Recupero l'ID dell'utente in modo sicuro
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // 3. Gestione del "Null": Se l'utente non è loggato, blocchiamo l'operazione
        if (userId == null)
        {
            return Unauthorized("Devi essere loggato per aggiungere un esame.");
        }

        // Ora il compilatore è sicuro che userId non è null
        exam.UserId = userId;

        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetExam", new { id = exam.Id }, exam);
    }
    // 3. Livello di Stress (Filtra per sicurezza che l'esame appartenga all'utente)
    [HttpGet("{id}/stress-level")]
    public async Task<ActionResult<object>> GetStressLevel(int id, [FromServices] PlannerService planner)
    {
        var userId = GetUserId();
        var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        
        if (exam == null) return NotFound("Esame non trovato o accesso non autorizzato.");

        int days = planner.GetDaysRemaining(exam.ExamDate);
        double hoursPerDay = exam.EstimatedTotalHours / (days > 0 ? days : 1);

        return new {
            ExamTitle = exam.Title,
            Status = hoursPerDay > 4 ? "Rosso (Urgente)" : hoursPerDay > 2 ? "Giallo (Attenzione)" : "Verde (Tranquillo)"
        };
    }

    // 4. Piano Giornaliero Adattivo
    [HttpGet("{id}/piano-giornaliero")]
    public async Task<ActionResult<object>> GetDailyPlan(int id, [FromServices] PlannerService planner)
    {
        var userId = GetUserId();
        var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        
        if (exam == null) return NotFound();

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
        var userId = GetUserId();
        var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        
        if (exam == null) return NotFound();

        _context.Exams.Remove(exam);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 6. Riepilogo Globale (Calcolato solo sugli esami dell'utente)
    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetGlobalSummary([FromServices] PlannerService planner)
    {
        var userId = GetUserId();
        var exams = await _context.Exams.Where(e => e.UserId == userId).ToListAsync();
        double totalHoursToday = 0;

        foreach (var exam in exams)
        {
            int daysLeft = planner.GetDaysRemaining(exam.ExamDate);
            if (daysLeft > 0)
            {
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
        var userId = GetUserId();
        var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        
        if (exam == null) return NotFound();

        exam.CompletedHours += hours;
        await _context.SaveChangesAsync();
        return Ok(exam.CompletedHours);
    }

    // 8. Reset Ore
    [HttpPost("{id}/reset-hours")]
    public async Task<IActionResult> ResetStudyHours(int id)
    {
        var userId = GetUserId();
        var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        
        if (exam == null) return NotFound();

        exam.CompletedHours = 0;
        await _context.SaveChangesAsync();
        return Ok();
    }
}