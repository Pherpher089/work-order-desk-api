using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Infrastructure.Persistence;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;
using WorkOrderDesk.Infrastructure.Persistence.Repositories;
using WorkOrderDesk.Api.WorkOrders;
using workOrderDesk.Api.WorkOrders;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger (OpenAPI) for .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WorkOrderDeskContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("WorkOrderDesk") ?? throw new InvalidOperationException("Connection string 'WorkOrderDesk' not found.");
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CreateWorkOrderHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/work-orders", async (
    CreateWorkOrderRequest request,
    CreateWorkOrderHandler handler,
    CancellationToken cancellationToken
) =>
{
    CreateWorkOrderCommand command = new CreateWorkOrderCommand
    {
        Title = request.Title,
        Description = request.Description,
        Priority = request.Priority
    };

    CreateWorkOrderResult result = await handler.HandleAsync(command, cancellationToken);

    return Results.Created($"/work-orders/{result.Id}", new CreateWorkOrderResponse
    {
        Id = result.Id,
        Title = result.Title,
        Description = result.Description,
        Priority = result.Priority,
        Status = result.Status,
        CreatedAtUtc = result.CreateAtUtc
    });

});

app.Run();
