using Business.Implement;
using Business.Interfaz;
using FluentValidation;
using FluentValidation.AspNetCore;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Web.ServiceExtension;
using Microsoft.OpenApi.Models; 
using Swashbuckle.AspNetCore.SwaggerUI;
using Data.Interfaz;
using Data.Implements.BaseData;


var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddSingleton<IValidatorFactory>(sp => new ServiceProviderValidatorFactory(sp));

// Swagger
builder.Services.AddSwaggerDocumentation();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Scoped services
builder.Services.AddScoped(typeof(IBaseData<>), typeof(BaseData<>));
builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));
// builder.Services.AddScoped<IPacienteData, PacienteData>();



// CORS
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(";");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origenesPermitidos)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware manejo errores personalizado (corregido JSON)
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Error interno del servidor."
        };
        var json = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    });
});

// Aplicar migraciones con logging


// CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        dbContext.Database.Migrate();
        logger.LogInformation("Base de datos verificada y migraciones aplicadas exitosamente.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la migración de la base de datos.");
    }
}
app.Run();