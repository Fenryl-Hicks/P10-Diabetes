using System.Text;
using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Repositories;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
using IdentityService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// ?? Configuration EF Core + SQL Server
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// ?? Identity + JWT
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<JwtOptions>(config.GetSection("Jwt"));

var jwt = config.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SigningKey"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = key
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

// Seed user (dev only)
using (var scope = app.Services.CreateScope())
{
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var user = await userMgr.FindByNameAsync("practitioner@clinic.test");
    if (user is null)
    {
        var newUser = new IdentityUser
        {
            UserName = "practitioner@clinic.test",
            Email = "practitioner@clinic.test",
            EmailConfirmed = true
        };
        await userMgr.CreateAsync(newUser, "P@ssw0rd!");
    }
}

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
