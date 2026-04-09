using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Infrastructure.Persistence;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;
using WorkOrderDesk.Infrastructure.Persistence.Repositories;
using WorkOrderDesk.Api.WorkOrders;
using WorkOrderDesk.Application.WorkOrders.ListWorkOrders;
using WorkOrderDesk.Api.Middleware;
using WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("WorkOrderDeskWeb", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CreateWorkOrderHandler>();
builder.Services.AddScoped<ListWorkOrdersHandler>();
builder.Services.AddScoped<GetWorkOrderByIdHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("WorkOrderDeskWeb");
app.UseMiddleware<ExceptionHandlingMiddleware>();

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

    return Results.Created($"/work-orders/{result.Id}", new CreateWorkOrderResult
    {
        Id = result.Id,
        Title = result.Title,
        Description = result.Description,
        Priority = result.Priority,
        Status = result.Status,
        CreatedAtUtc = result.CreatedAtUtc
    });

});

app.MapGet("/work-orders", async (
    ListWorkOrdersHandler handler,
    CancellationToken cancellationToken
) =>
{
    IReadOnlyList<WorkOrderListItem> items = await handler.HandleAsync(new ListWorkOrdersQuery(), cancellationToken);

    IEnumerable<WorkOrderListItemResponse> response = items.Select(x => new WorkOrderListItemResponse
    {
        Id = x.Id,
        Title = x.Title,
        Description = x.Description,
        Priority = x.Priority,
        Status = x.Status,
        CreatedAtUtc = x.CreatedAtUtc,
        UpdatedAtUtc = x.UpdatedAtUtc
    });

    return Results.Ok(response);
});

app.MapGet("/work-orders/{id:guid}", async (
    Guid id,
    GetWorkOrderByIdHandler handler,
    CancellationToken cancellationToken
) =>
{
    var result = await handler.HandleAsync(new GetWorkOrderByIdQuery
    {
        Id = id
    }, cancellationToken);

    if (result is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new WorkOrderDetailsResponse
    {
        Id = result.Id,
        Title = result.Title,
        Description = result.Description,
        Priority = result.Priority,
        Status = result.Status,
        AssigneeId = result.AssigneeId,
        CreatedAtUtc = result.CreatedAtUtc,
        UpdatedAtUtc = result.UpdatedAtUtc,
        CompltedAtUtc = result.CompletedAtUtc,
        ArchivedAtUtc = result.ArchivedAtUtc
    });
});

app.Run();
