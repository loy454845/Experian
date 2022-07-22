using ExperianTechTest.Infrastructure;
using ExperianTechTest.Models;
using ExperianTechTest.Services;
using ExperianTechTest.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<IFormFile>, FileValidator>();
builder.Services.AddScoped<IFileHelper, FileHelper>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IFinancialService, FinancialService>();

builder.Services.AddScoped<ICsvToObject<Person>, CsvToObject<Person>>();
builder.Services.AddScoped<ICsvToObject<Finance>, CsvToObject<Finance>>();

builder.Services.AddScoped<IValidator<Person>, PersonValidator>();
builder.Services.AddScoped<IValidator<Finance>, FinanceValidator>();

builder.Services.AddCosmosRepository(
        options =>
        {
            options.CosmosConnectionString = builder.Configuration.GetConnectionString("ExperianAzureDbConn");
            options.ContainerId = "personrecords";
            options.DatabaseId = "persondb";
        });

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        a => a.WithOrigins("http://localhost:3000"));
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExperianTechTest v1"));
}

app.UseCors(options =>
{
    options.
    WithOrigins("http://localhost:3000", "").
    AllowAnyMethod().
    AllowAnyHeader();
});
app.UseCors("MyPolicy");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();