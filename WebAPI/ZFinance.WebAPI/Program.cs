using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;
using ZFinance.Core;
using ZFinance.Core.ExtensionMethods;
using ZFinance.WebAPI.ExtensionMethods;

string version = "0.0.0";
if (Assembly.GetEntryAssembly()?.GetName()?.Version is Version assemblyVersion)
{
    version = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Revision}";
}

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
#endregion

#region Database
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddDbContext<IDbContext, AppDbContext>(options => options
        .UseLazyLoadingProxies()
        .UseSqlServer(
            connectionString,
            x =>
            {
                x.MigrationsAssembly(typeof(AppDbContext).Assembly);
                x.MigrationsHistoryTable("MigrationsHistory", nameof(EF));
            }
        )
    );
#endregion

#region Authorization & Authentication
builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build()
        );
    })
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
        };
    });
#endregion

#region Repositories, Services, and Jobs
builder.Services
    .AddHttpContextAccessor();

builder.Services
    // System
    .AddRepositories()
    .AddCoreServices()
    .AddWebAPIServices();
#endregion

#region AutoMapper
builder.Services
    .AddAutoMapper(typeof(Program).Assembly);
#endregion

#region Swagger
builder.Services
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Contact = new OpenApiContact { Name = "Ricardo Zambon", Url = new Uri("https://github.com/RicardoZambon") },
            Title = "ZFinance WebApi",
            Version = "v1",
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    }
                },
                Array.Empty<string>()
            }
        });

        string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
#endregion

#region Logging / Sentry
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration);

builder.Logging.AddSentry(options =>
{
    options.AttachStacktrace = true;
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.EnableLogs = true;
    options.Environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];
    options.Release = $"v@{version}";
    options.SendDefaultPii = true;
    options.StackTraceMode = StackTraceMode.Enhanced;
});

#if DEBUG
builder.Logging.AddConsole();
#endif
#endregion


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply database migrations during runtime.
using IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
if (scope.ServiceProvider.GetService<IDbContext>() is IDbContext dbContext)
{
    ICurrentUserProvider<long>? currentUserProvider = scope.ServiceProvider.GetService<ICurrentUserProvider<long>>();
    try
    {
        currentUserProvider?.EnableServiceUserMode();
        dbContext.ApplyDatabaseInitializations();
    }
    catch
    {
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
    }
    finally
    {
        currentUserProvider?.DisableServiceUserMode();
    }
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSentryTracing();

app.UseCors();

app.Run();