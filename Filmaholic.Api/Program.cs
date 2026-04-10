using Filmaholic.Api.Classes;
using Filmaholic.Api.Data;
using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add EF Core + SQLite
builder.Services.AddDbContext<MoviesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MoviesDb")));
builder.Services.AddScoped<IMovie, MovieClass>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.MapMovieEndpoints();
//app.UseHttpsRedirection();

app.Run();