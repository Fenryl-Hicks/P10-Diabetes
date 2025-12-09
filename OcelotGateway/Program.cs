using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Détecter l'environnement Docker et charger le bon fichier ocelot.json
var ocelotConfigFile = builder.Environment.EnvironmentName == "Docker" 
    ? "ocelot.Docker.json" 
    : "ocelot.json";

builder.Configuration.AddJsonFile(ocelotConfigFile, optional: false, reloadOnChange: true);

// Authentification JWT pour Ocelot
builder.Services.AddAuthentication("GatewayAuthenticationScheme")
    .AddJwtBearer("GatewayAuthenticationScheme", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "P10.Diabetes.Auth",
            ValidAudience = "P10.Diabetes.API",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("super_secret_dev_key_change_me_please_32_chars_min"))
        };
    });

// Ajouter Ocelot
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
