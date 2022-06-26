using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stronzo.PickToLight.Api.Configurations;
using Stronzo.PickToLight.Api.Extensions;
using Stronzo.PickToLight.Api.Middlewares;
using Stronzo.PickToLight.Api.Models;
using Stronzo.PickToLight.Api.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.LoadConfiguration();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appConfig.Authentication.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton<ApplicationConfig>(appConfig);
builder.Services.AddSingleton<IActionResultConverter, ActionResultConverter>();

builder.Services.AddValidators();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUseCases();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(config => config.LowercaseUrls = true);

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api-docs/{documentName}/open-api.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v1/open-api.json", "Stronzo.PickToLight.Api v1");
    c.RoutePrefix = "api-docs";
});

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
