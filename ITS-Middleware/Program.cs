using ITS_Middleware.Helpers.Log;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddLogging(config => config.AddLog4Net("log4net.config", true));

builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true, reloadOnChange: true);
ITS_Middleware.Constants.Vars.API_URI = builder.Configuration.GetSection("ApplicationSettings:ApiSettings:DefaultUri").Value.ToString();


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
