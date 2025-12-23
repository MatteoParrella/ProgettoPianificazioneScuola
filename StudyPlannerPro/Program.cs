using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Services; // Assicurati che questo namespace sia corretto

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=studyplanner.db"));

// Registrazione del servizio per l'algoritmo
builder.Services.AddScoped<PlannerService>(); 

var app = builder.Build();

// 2. MIDDLEWARE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Spostiamo i file statici PRIMA di tutto il resto
app.UseDefaultFiles(); 
app.UseStaticFiles();

// Commentiamo questa se hai ancora l'errore "Failed to determine https port"
// app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();