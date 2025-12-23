using System.ComponentModel.DataAnnotations;

namespace StudyPlannerPro.Models;

public class Subject 
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string ColorHex { get; set; } = "#3498db"; 

    // Deve essere string per essere compatibile con IdentityUser
    public string UserId { get; set; } = string.Empty; 
}