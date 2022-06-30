using Microsoft.EntityFrameworkCore;
using OauthAPI.Models.Context;
var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true, reloadOnChange: true);
OauthAPI.Constants.Vars.CONNECTION_STRING = builder.Configuration.GetSection("ApplicationSettings:ConnectionStrings:DefaultConnection").Value.ToString();

builder.Services.AddDbContext<OauthContextDb>(options =>
{
    options.UseSqlServer(OauthAPI.Constants.Vars.CONNECTION_STRING);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
