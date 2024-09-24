using ECommerce.App.Database.Entities;
using NSwag.Generation.Processors.Security;
using NSwag;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.App.Database;
using ECommerce.App.Services;
using ECommerce.Shared.Entities;
using DeltaX.Core.Common;
using DeltaX.Core.Hosting.Auth;
using DeltaX.Core.Hosting.Remote;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.SchemaSettings.GenerateKnownTypes = true;
    c.DocumentProcessors.Add(new AddAdditionalTypeProcessorAssembly(typeof(ProductDto).Assembly));
    c.OperationProcessors.Add(new OperationSecurityScopeProcessor());
    c.DocumentProcessors.Add(new SecurityDefinitionAppender("Bearer",
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            Description = "Copy 'Bearer ' + valid JWT token into field",
            In = OpenApiSecurityApiKeyLocation.Header,
            Scheme = IdentityConstants.BearerScheme
        }));
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddClaimsPrincipalFactory<ApplicationClaimsFactory>()
    .AddApiEndpoints();

var connectionString = builder.Configuration.GetConnectionString("DbSqlServer")
    ?? throw new InvalidOperationException("Connection string 'DbSqlServer' not found.");

builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    options.EnableSensitiveDataLogging();
    options.UseSqlServer(connectionString);
});
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSingleton<MapperService>();
builder.Services.AddSingleton<EventBus>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentUser>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddSingleton(s => s.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddSignalR();

var app = builder.Build();

app.EnsureDatabaseCreated<SecurityDbContext>(true);
app.EnsureDatabaseCreated<ECommerceDbContext>(true);

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

//app.UseHttpsRedirection();

app.MapGroup("/security").MapIdentityApi<User>();

app.UsePathBase(app.Configuration.GetValue("RequestPathBase", "/ecommerce/"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.MapGroup("/ecommerce/ui").MapFallbackToFile("index.html");

app.Run();
