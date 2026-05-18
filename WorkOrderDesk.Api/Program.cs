using Microsoft.EntityFrameworkCore;
using WorkOrderDesk.Infrastructure.Persistence;
using WorkOrderDesk.Application.Abstractions;
using WorkOrderDesk.Application.WorkOrders.CreateWorkOrder;
using WorkOrderDesk.Infrastructure.Persistence.Repositories;
using WorkOrderDesk.Api.WorkOrders;
using WorkOrderDesk.Application.WorkOrders.ListWorkOrders;
using WorkOrderDesk.Api.Middleware;
using WorkOrderDesk.Application.WorkOrders.GetWorkOrderById;
using WorkOrderDesk.Application.WorkOrders.UpdateWorkOrder;
using WorkOrderdeks.Api.WorkOrders;
using WorkOrderDesk.Application.WorkOrders.DeleteWorkOrder;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();

// Swagger (OpenAPI) for .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("WorkOrderDesk") ?? throw new InvalidOperationException("Connection string 'WorkOrderDesk' not found.");
builder.Services.AddDbContext<WorkOrderDeskContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var allowedOrigins = builder.Configuration["AllowedOrigins"]
    ?? throw new InvalidOperationException("AllowedOrigins configuration is missing.");

string[] origins = allowedOrigins
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("WorkOrderDeskWeb", policy =>
    {
        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});




builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CreateWorkOrderHandler>();
builder.Services.AddScoped<ListWorkOrdersHandler>();
builder.Services.AddScoped<GetWorkOrderByIdHandler>();
builder.Services.AddScoped<UpdateWorkOrderHandler>();
builder.Services.AddScoped<DeleteWorkOrderHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WorkOrderDeskContext>();
    dbContext.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("WorkOrderDeskWeb");
app.UseMiddleware<ExceptionHandlingMiddleware>();

// app.MapControllers();

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

app.MapPut("/work-orders/{id:guid}", async (
    Guid id,
    UpdateWorkOrderRequest request,
    UpdateWorkOrderHandler handler,
    CancellationToken cancellationToken
) =>
{
    var command = new UpdateWorkOrderCommand
    {
        Id = id,
        Title = request.Title,
        Description = request.Description,
        Priority = request.Priority,
        Status = request.Status,
        AssigneeId = request.AssigneeId
    };

    var result = await handler.HandleAsync(command, cancellationToken);

    if (result is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new UpdateWorkOrderResponse
    {
        Id = result.Id,
        Title = result.Title,
        Description = result.Description,
        Priority = result.Priority,
        Status = result.Status,
        AssigneeId = result.AssigneeId,
        CreatedAtUtc = result.CreatedAtUtc,
        UpdateAtUtc = result.UpdatedAtUtc,
        CompletedAtUtc = result.CompletedAtUtc,
        ArchivedAtUtc = result.ArchivedAtUtc
    });
});

app.MapDelete("/work-orders/{id:guid}", async (
    Guid id,
    DeleteWorkOrderHandler handler,
    CancellationToken cancellationToken
) =>
{
    var deleted = await handler.HandleAsync(new DeleteWorkOrderCommand
    {
        Id = id
    }, cancellationToken);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});


app.Run();
