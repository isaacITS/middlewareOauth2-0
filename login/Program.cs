using login.Constants;
using login.Helpers.Log;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true, reloadOnChange: true);
VarsHelpers.API_URI = builder.Configuration.GetSection("ApplicationSettings:ApiSettings:DefaultUri").Value.ToString();

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddLogging(config => config.AddLog4Net("log4net.config", true));
VarsHelpers.ApiKey = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:apiKey").Value.ToString();
VarsHelpers.AuthDomain = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:authDomain").Value.ToString();
VarsHelpers.ProjectId = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:projectId").Value.ToString();
VarsHelpers.StorageBucket = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:storageBucket").Value.ToString();
VarsHelpers.MessagingSenderId = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:messagingSenderId").Value.ToString();
VarsHelpers.AppId = builder.Configuration.GetSection("ApplicationSettings:firebaseSecret:appId").Value.ToString();

var app = builder.Build();

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
    pattern: "{controller=Home}/{action=SignIn}/{id?}");

app.Run();
