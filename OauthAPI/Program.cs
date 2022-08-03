using OauthAPI.Models.Context;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using OauthAPI.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;

// Add services to the container.
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: false, reloadOnChange: true);
OauthAPI.Constants.Vars.CONNECTION_STRING = builder.Configuration.GetSection("ApplicationSettings:ConnectionStrings:DefaultConnection").Value.ToString();


string SecretKey = builder.Configuration.GetSection("ApplicationSettings:Jwt:SecretKey").Value.ToString();
string Issuer = builder.Configuration.GetSection("ApplicationSettings:Jwt:Issuer").Value.ToString();
string Audience = builder.Configuration.GetSection("ApplicationSettings:Jwt:Audience").Value.ToString();

SymmetricSecurityKey SecuritySecretKey = new(Encoding.UTF8.GetBytes(SecretKey));

builder.Services.AddDbContext<OauthContextDb>(options =>
{
    options.UseSqlServer(OauthAPI.Constants.Vars.CONNECTION_STRING);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtOauth20";
    options.DefaultChallengeScheme = "JwtOauth20";
})
    .AddJwtBearer("JwtOauth20", jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = SecuritySecretKey,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            ValidateLifetime = true
        };
    });

var fileSecret = Path.Combine(_env.ContentRootPath, @"FirebaseAdminSecret.json");

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(fileSecret)
});


FirebaseHelper fbHelper = new();

await fbHelper.CreateBaseUser(
    builder.Configuration.GetSection("ApplicationSettings:FirebaseStorage:email").Value.ToString(),
    builder.Configuration.GetSection("ApplicationSettings:FirebaseStorage:key").Value.ToString(),
    builder.Configuration.GetSection("ApplicationSettings:FirebaseStorage:name").Value.ToString()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
