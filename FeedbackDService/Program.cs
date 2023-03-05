using FeedbackDService.Configs;
using FeedbackDService.Data.Context;
using FeedbackDService.Extensions;
using FeedbackDService.Mapper;
using FeedbackDService.Middlewares;
using FeedbackDService.Migrations;
using FeedbackDService.Services.Authentication;
using FeedbackDService.Services.FileSaveService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

AuthenticationConfig? authConfig = configuration.GetSection(AuthenticationConfig.SectionKey).Get<AuthenticationConfig>();
if (authConfig is null)
    throw new NullReferenceException("Can't parse auth config");

services.AddSingleton<AuthenticationConfig>(authConfig);
services.AddScoped<IAuthenticationService, AuthenticationService>();

services.AddAutoMapper(typeof(MapperProfile.Authorization));
services.AddAutoMapper(typeof(MapperProfile.CompanyCategoryProfile));
services.AddAutoMapper(typeof(MapperProfile.CompanyProfile));

string webRootPath = builder.Environment.WebRootPath;
services.AddScoped<ImageFileSaveService>(_ => new ImageFileSaveService
{
    WebRootDirectoryPath = webRootPath,
});

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ValidateIssuer = true,
        ValidateIssuer = false,
        // ValidIssuer = authConfig.Issuer,
        
        // ValidateAudience = true,
        ValidateAudience = false,
        // ValidAudience = authConfig.Audience,
        
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = authConfig.SymmetricSecurityKey(),
        
        ClockSkew = TimeSpan.Zero,
    };
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.UseCustomSwagger();

services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Database"));
});

var app = builder.Build();

app.UseRequestLogging();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(config =>
{
    config.AllowAnyMethod();
    config.AllowAnyOrigin();
    config.AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseClaimsDetermination();

app.MapControllers();

app.Run();