using System;

namespace StudyPlannerPro.Models;

public class User {
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    // Quante ore lo studente può dedicare allo studio ogni giorno
    public double DailyCapacityHours { get; set; } = 4.0; 
}
