using System;

namespace StudyPlannerPro.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Models;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/subjects (Per vedere tutte le materie)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
    {
        return await _context.Subjects.ToListAsync();
    }

    // POST: api/subjects (Per aggiungere una materia)
    [HttpPost]
    public async Task<ActionResult<Subject>> PostSubject(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSubjects), new { id = subject.Id }, subject);
    }
}
