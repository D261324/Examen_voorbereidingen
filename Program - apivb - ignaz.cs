using ApiBasicIgnaz;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetService<AppDbContext>();
//context.Database.EnsureDeleted();
context.Database.EnsureCreated();



//get
//-maak alle get routes aan: 
//    --get voor all usersInfo
//    -- get voor one specific user usersInfo
//    v get voor all articleItems
//    -- get voor one specific articleItems
//    x get route voor de google api?
//    v get route voor de een rol

//get all roles
app.MapGet("/roles", async () => await context.Roles.ToListAsync());

app.MapGet("/roles/{RolId}", async (int RolId, AppDbContext context) => await context.Roles.FindAsync(RolId));

// is working
app.MapGet("/archiveItems", async () => await context.Archives.ToListAsync());

//get route voor alle users
app.MapGet("/users", async () => await context.Users.ToListAsync());
//get route voor een users
app.MapGet("/users/{UserId}", async () => await context.Users.ToListAsync());

//post
//-maak alle post routes aan: 
//    -- post voor een nieuwe users
//    -- post voor een nieuwe articleItems
//    -- post voor een nieuwe wachtwoorden
//    - post voor de google api?
app.MapPost("/users", async (User user) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/users/{user.UserId}", user);
});
// make a route for a new archiveItems(werkt)
app.MapPost("/archiveItems", async (Archive archive) =>
{
    context.Archives.Add(archive);
    await context.SaveChangesAsync();
    return Results.Created($"/archiveItems/{archive.ArchiveId}", archive);
});
//make a route that gets one archive item by id
app.MapGet("/archiveItems/{ArchiveId}", async (int archiveId) =>
{
    var archive = await context.Archives.FindAsync(archiveId);
    if (archive == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(archive);
});



app.MapPost("/roles", async (Role role) =>
{
    context.Roles.Add(role);
    await context.SaveChangesAsync();
    return Results.Created($"/roles/{role.RoleId}", role);
});


//archief post route
app.MapPost("/archives", async (Archive archive) =>
{
    context.Archives.Add(archive);
    await context.SaveChangesAsync();
    return Results.Created($"/archives/{archive.ArchiveId}", archive);
});



//update
//-update voor nieuwe users
//    - update voor nieuwe articleItems
//    - update voor nieuwe wachtwoorden
app.MapPut("/users/{id}", async (int id, User user) =>
{
    var userToUpdate = await context.Users.FindAsync(id);
    if (userToUpdate == null)
    {
        return Results.NotFound();
    }
    userToUpdate.Password = user.Password;
    userToUpdate.PasswordRepeat = user.PasswordRepeat;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// make a route to update user passwords
app.MapPut("/users/{id}", async (int id, User user) =>
{
    var userToUpdate = await context.Users.FindAsync(id);
    if (userToUpdate == null)
    {
        return Results.NotFound();
    }
    userToUpdate.Password = user.Password;
    userToUpdate.PasswordRepeat = user.PasswordRepeat;
    await context.SaveChangesAsync();
    return Results.NoContent();
});





app.Run();
