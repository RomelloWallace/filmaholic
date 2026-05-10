using Filmaholic.Api.Classes;
using Filmaholic.Api.Data;
using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Filmaholic.Api.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MoviesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MoviesDb")));
builder.Services.AddScoped<IMovieService, MovieService>();
builder.WebHost.UseUrls("http://0.0.0.0:5220");
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        var problem = exception switch
        {
            NotFoundException ex => new ProblemDetails
            {
                Title = "Resource not found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            },

            ConflictException ex => new ProblemDetails
            {
                Title = "Conflict",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            },

            UnauthorizedAccessException => new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized
            },

            _ => new ProblemDetails
            {
                Title = "Server error",
                Status = StatusCodes.Status500InternalServerError
            }
        };

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(problem);
    });
});
app.MapMovieEndpoints();

app.Run();