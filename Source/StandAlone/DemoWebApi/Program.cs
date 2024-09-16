using DemoWebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Security;
using NSwag;
using System.Text;
using System.Reflection;
using Namotion.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using DbUp;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Connections;
using DemoWebApi.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.SchemaSettings.GenerateKnownTypes = true;
    // c.DocumentProcessors.Add(new AddAdditionalTypeProcessorAssembly(typeof(Program).Assembly));
    c.OperationProcessors.Add(new OperationSecurityScopeProcessor());
    c.DocumentProcessors.Add(new SecurityDefinitionAppender("Bearer",
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            Description = "Copy 'Bearer ' + valid JWT token into field",
            In = OpenApiSecurityApiKeyLocation.Header,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        }));
});

// here we register our service
builder.Services.AddTransient<AuthJwtService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });
// builder.Services
//     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(opt =>
//     {
//         var key = builder.Configuration.GetValue<string>("Jwt:Key")!;
//         var issuer = builder.Configuration.GetValue<string>("Jwt:ValidIssuer");
//         var audience = builder.Configuration.GetValue<string>("Jwt:ValidAudience");

//         opt.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = issuer,
//             ValidAudience = audience,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//         };
//     });

var connectionString = builder.Configuration.GetConnectionString("DbSqlServer")
    ?? throw new InvalidOperationException("Connection string 'DbSqlServer' not found.");

builder.Services.AddScoped<IDbConnection>(e => new SqlConnection(connectionString));
builder.Services.AddScoped<AuthRepository>();

// DB Migration
Migration.PerformUpgrade(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

// Default CORS
app.UseCors(builder => builder
    .WithOrigins("http://127.0.0.1:4200", "http://localhost:5173")
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UsePathBase(app.Configuration.GetValue("RequestPathBase", "/demo/"));

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

//app.MapGroup("/demo/ui").MapFallbackToFile("index.html");

app.Run();
