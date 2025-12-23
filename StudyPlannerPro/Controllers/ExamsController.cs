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
}
