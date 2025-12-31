using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure.Data;
using Recipes.Infrastructure.Interfaces;
using Recipes.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=recipes.db"));

//Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              //.AllowAnyOrigin() // Change prod with Vercel URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Register AutoMapper
builder.Services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies().ToArray());

//Register services
builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

//Set CORS before MapController
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
