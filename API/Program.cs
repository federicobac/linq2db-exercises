using System.Text.Json.Serialization;
using API;
using Microsoft.AspNetCore.Mvc;
using Infa;
using LinqToDB;

var builder = WebApplication.CreateBuilder(args);

var options = new DataOptions().UseSQLite("Data Source=dev.db");
builder.Services.AddSingleton(new DataOptions<GroceryDatabase>(options));
builder.Services.AddScoped<GroceryDatabase>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemExceptionHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApiDocument();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GroceryDatabase>();
    GrocerySeed.EnsureSeeded(db);
}

app.UseExceptionHandler();
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.Run();
