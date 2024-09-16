using DeltaX.Core.Common;
using DeltaX.Core.Hosting.Auth;
using DeltaX.Core.Hosting.Remote;
using DemoBlazor.Client.Shared;
using DemoBlazor.Components;
using DemoBlazor.Database;
using DemoBlazor.Database.Entities;
using DemoBlazor.Services;
using DemoBlazor.Shared.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using NSwag.Generation.Processors;
using Radzen;
using System.Security.Claims;
using DemoBlazor.Shared.Events;
using System.Reflection;
using YamlDotNet.Serialization;
using NSwag.Generation.Processors.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.SchemaSettings.GenerateKnownTypes = true;
    c.DocumentProcessors.Add(new AddAdditionalTypeProcessorAssembly(typeof(TourCreated).Assembly));
});
//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddWebhook<PaymentNotification>("application/json");
//    c.AddWebhook<PullRequestMergedNotification>("application/x-www-form-urlencoded");
//}

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<IdentityUserAccessor>();
//builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DemoBlazorDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddSignInManager()
    .AddClaimsPrincipalFactory<ApplicationClaimsFactory>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<MapperService>();
builder.Services.AddSingleton<EventBus>();
builder.Services.AddHostedService<EventBusToMediatorHub>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentUser>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ClientState>();
builder.Services.AddSingleton<AuthClientService>();
builder.Services.AddSingleton(s => s.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

builder.Services.AddCascadingAuthenticationState();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddSignalR();

var app = builder.Build();

app.EnsureDatabaseCreated<SecurityDbContext>(true);
app.EnsureDatabaseCreated<DemoBlazorDbContext>(true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGroup("/security").MapIdentityApi<ApplicationUser>();
app.MapGroup("/security").MapPost("/Logout", async (
                SignInManager<ApplicationUser> signInManager,
                [FromForm] string returnUrl) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.LocalRedirect($"{returnUrl}");
});

app.MapHub<MediatorHubServer>(MediatorHubServer.EndpointName);

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api").MapControllers();

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DemoBlazor.Client._Imports).Assembly);

app.Run();

public sealed class AddAdditionalTypeProcessorAssembly : IDocumentProcessor
{
    private List<ContextualType> contextualTypes;

    public AddAdditionalTypeProcessorAssembly(params Assembly[] assemblies)
    {
        contextualTypes = assemblies
            .SelectMany(t => { try { return t.GetTypes(); } catch { return []; } })
            .Select(t => t.ToContextualType())
            .Distinct()
            .ToList();
    }

    public void Process(DocumentProcessorContext context)
    {
        foreach (var type in contextualTypes)
        {
            context.SchemaGenerator.Generate(type, context.SchemaResolver);
        }
    }
}
