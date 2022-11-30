using Microsoft.EntityFrameworkCore;
using WorldCupSimulator.Contexts;
using WorldCupSimulator.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WorldCupContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/teams/{id}", async (WorldCupContext context, Guid id) =>
{
    try
    {
        var team = await context.Teams.FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
            return Results.NotFound("Not Found");

        return Results.Ok(team);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/teams", async (WorldCupContext context) =>
{
	try
	{
        var teams = await context.Teams.ToListAsync();
        return Results.Ok(teams);
	}
	catch (Exception ex)
	{
        return Results.BadRequest(ex.Message);
	}
});

app.MapPost("/teams", async (WorldCupContext context, Team team) =>
{
    try
    {
        await context.Teams.AddAsync(team);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/teams", async (WorldCupContext context, Team team) =>
{
    try
    {
        context.Teams.Update(team);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/teams/{id}", async (WorldCupContext context, Guid id) =>
{
    try
    {
        var team = await context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        if (team != null)
        {
            context.Teams.Remove(team);
            await context.SaveChangesAsync();
            return Results.Ok();
        }

        return Results.NotFound("Not Found");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();