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
        double load = planner.CalculateStudyLoad(exam.Difficulty, exam.Importance, days);

        return new {
            ExamTitle = exam.Title,
            DaysRemaining = days,
            StressScore = load,
            Status = load > 5 ? "Rosso (Urgente)" : load > 2 ? "Giallo (Attenzione)" : "Verde (Tranquillo)"
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
}
