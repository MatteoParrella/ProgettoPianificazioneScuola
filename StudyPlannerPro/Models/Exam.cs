using System;

namespace StudyPlannerPro.Models;

public class Exam {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    
    // Valori da 1 a 5 per l'algoritmo
    public int Difficulty { get; set; } 
    public int Importance { get; set; }
    
    public int SubjectId { get; set; }
    public int UserId { get; set; }
    
    // Stato di avanzamento
    public bool IsCompleted { get; set; } = false;
    public double EstimatedTotalHours { get; set; } // Ore totali stimate per prepararsi
}
