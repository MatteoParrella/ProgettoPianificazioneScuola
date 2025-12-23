using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyPlannerPro.Data;
using StudyPlannerPro.Services;
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICES ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=studyplanner.db"));

builder.Services.AddScoped<PlannerService>(); 

// Configurazione Identity API (Gestisce tutto lei: Auth, Cookies e Token)
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>();

// L'autorizzazione è necessaria per proteggere i controller con [Authorize]
builder.Services.AddAuthorization();

var app = builder.Build();

// --- 2. MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles(); 
app.UseStaticFiles();

// Fondamentale: Authentication prima di Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

// Mappiamo le rotte sotto il gruppo /auth
app.MapGroup("/auth").MapIdentityApi<IdentityUser>(); 

app.Run();