using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Aggiungi il supporto per i Controller (fondamentale per le API)
builder.Services.AddControllers();

// 2. Configurazione Swagger/OpenAPI (per testare le API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Più comune di AddOpenApi per iniziare

// 3. Il tuo database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=studyplanner.db"));

builder.Services.AddScoped<StudyPlannerPro.Services.PlannerService>();

var app = builder.Build();

// Configura la pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Attiva l'interfaccia grafica per testare le API
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. Mappa i controller (questo sostituisce i MapGet manuali)
app.MapControllers();

app.Run();