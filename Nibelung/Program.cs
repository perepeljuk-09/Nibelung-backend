using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nibelung.Api.Services;
using Nibelung.Api.Services.Contracts;
using Nibelung.Infrastructure.Configs;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Helpers;
using Nibelung.Infrastructure.Helpers.Contract;
using Nibelung.Infrastructure.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configBuilder = new ConfigurationBuilder();


configBuilder.AddJsonFile("appsettings.json");
var config = configBuilder.Build();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<PgContext>(options =>
    options.UseNpgsql(config.GetConnectionString("pg")))
    .AddTransient<IAuthService, AuthService>()
    .AddTransient<IUserService, UserService>()
    .AddTransient<IJwtService, JwtService>()
    .AddScoped<IUserContext, UserContext>()
    .AddScoped<IPostService, PostService>()
    .AddScoped<IPostLikeService, PostLikeService>()
    .AddScoped<ICommentService, CommentService>()
    .AddScoped<IFileService, FileService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = JwtTokenConfig.SecurityKey,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "NibelungAPI", Version = "v1.1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please enter token // Bearer + token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header,
                Description = "Bearer token"
            },
            new string[]{}
        }
    });
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(opt => opt.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition")
        .SetIsOriginAllowed((_) => true).Build());

app.MapControllers();

app.Run();
