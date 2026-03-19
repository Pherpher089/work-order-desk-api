using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger (OpenAPI) for .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WorkOrderDeskContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("WorkOrderDesk");
    options.UseSqlite(connectionString);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
