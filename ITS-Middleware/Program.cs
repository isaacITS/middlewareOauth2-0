using ITS_Middleware.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSession();


builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true, reloadOnChange: true);
ITS_Middleware.Constants.Vars.CONNECTION_STRING = builder.Configuration.GetSection("ApplicationSettings:ConnectionStrings:DefaultConnection").Value.ToString();

builder.Services.AddDbContext<middlewareITSContext>(options =>
{
    options.UseSqlServer(ITS_Middleware.Constants.Vars.CONNECTION_STRING);
});



var app = builder.Build();

app.UseSession();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
