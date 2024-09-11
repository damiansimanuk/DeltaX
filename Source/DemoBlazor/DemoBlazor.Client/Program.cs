using DeltaX.Core.Client.Remote;
using DemoBlazor.Client.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddRadzenComponents();
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = baseAddress });
builder.Services.AddSingleton(sp => new MediatorHubClient(baseAddress));
builder.Services.AddSingleton<ClientState>();
builder.Services.AddSingleton<AuthClientService>();
builder.Services.AddSingleton(s => s.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
