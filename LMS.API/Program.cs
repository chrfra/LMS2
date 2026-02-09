using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Ensure data directory exists
var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// Add services to the container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString != null && connectionString.Contains("data/"))
{
    connectionString = connectionString.Replace("data/", $"{dataDir}/");
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IScormParserService, ScormParserService>();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-jwt-key-change-this-in-production";
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["ValidAudience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();
    await DbInitializer.InitializeAsync(dbContext, passwordService);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

