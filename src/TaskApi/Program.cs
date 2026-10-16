using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Dtos;
using TaskApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Configuração de Serviços
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Execução automática de Migrations ao iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configuração do Pipeline HTTP
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Healthcheck
app.MapHealthChecks("/health");

// --- ENDPOINTS DA API ---

// GET /api/tasks - Listar todas
app.MapGet("/api/tasks", async (AppDbContext db) =>
{
    var tasks = await db.Tasks.AsNoTracking().ToListAsync();
    return Results.Ok(tasks);
})
.WithName("GetAllTasks")
.WithTags("Tasks");

// GET /api/tasks/{id} - Buscar por ID
app.MapGet("/api/tasks/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound(new { message = "Tarefa não encontrada." });
})
.WithName("GetTaskById")
.WithTags("Tasks");

// POST /api/tasks - Criar nova
app.MapPost("/api/tasks", async (CreateTaskDto dto, AppDbContext db) =>
{
    var validation = TaskItem.ValidateTitle(dto.Title);
    if (!validation.IsValid)
    {
        return Results.BadRequest(new { error = validation.ErrorMessage });
    }

    var task = new TaskItem
    {
        Title = dto.Title,
        Description = dto.Description
    };

    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/api/tasks/{task.Id}", task);
})
.WithName("CreateTask")
.WithTags("Tasks");

// PUT /api/tasks/{id} - Atualizar
app.MapPut("/api/tasks/{id:guid}", async (Guid id, UpdateTaskDto dto, AppDbContext db) =>
{
    var validation = TaskItem.ValidateTitle(dto.Title);
    if (!validation.IsValid)
    {
        return Results.BadRequest(new { error = validation.ErrorMessage });
    }

    var task = await db.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound(new { message = "Tarefa não encontrada." });
    }

    task.Title = dto.Title;
    task.Description = dto.Description;
    task.IsCompleted = dto.IsCompleted;

    await db.SaveChangesAsync();
    return Results.Ok(task);
})
.WithName("UpdateTask")
.WithTags("Tasks");

// DELETE /api/tasks/{id} - Remover
app.MapDelete("/api/tasks/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound(new { message = "Tarefa não encontrada." });
    }

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteTask")
.WithTags("Tasks");

app.Run();