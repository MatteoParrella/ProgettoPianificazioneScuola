using System;

namespace StudyPlannerPro.Models;

public class Subject {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#3498db"; // Colore per UI
    public int UserId { get; set; } // Foreign Key
}
