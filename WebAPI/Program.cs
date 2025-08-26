using Application.Utilities;
using Infrastructure.Persistence.Configurations;
using Infrastructure.Persistence.Seeds;
//using Infrastructure.Persistence.StoredProcedures;
using Microsoft.EntityFrameworkCore;
using WebAPI;


var builder = WebApplication.CreateBuilder(args);


// Inyección de DbContext
builder.Services.AddDbContext<ProjectContext>();

// Inyección de servicios
builder.Services.AddServices();

// Inyección de repositorios
builder.Services.AddRepositories();

// Inyección de endpoints
builder.Services.AddControllers();

// Inyección de logger
builder.Services.AddSingleton<IProjectLogger, ProjectLogger>();

// Configuracion de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVPNRange", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            origin.StartsWith("http://10.8.0.") ||
            origin.StartsWith("https://10.8.0.") ||
            origin.StartsWith("http://localhost") ||
            origin.StartsWith("https://localhost"))
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Usar siempre la política (en dev y prod)
app.UseCors("AllowVPNRange");

// Acá se agregó la creación y migración automática de la base de datos
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ProjectContext>();
    var logger = serviceScope.ServiceProvider.GetRequiredService<IProjectLogger>();
    context.Database.Migrate();
    logger.LogInformation("Base de datos migrada correctamente.");
    await SeedManager.InitializeAsync(context, logger); // Orquestadora de Seeds
    //await SPManager.InitializeAsync(context, logger); // Orquestadora de Stored Procedures
}
// Acá hubo que adaptar la compatibilidad de la versión de Swagger
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
app.UseSwaggerUI();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
