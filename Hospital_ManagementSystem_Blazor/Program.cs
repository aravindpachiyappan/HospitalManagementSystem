using Blazored.LocalStorage;
using Hospital_ManagementSystem_Blazor;
using Hospital_ManagementSystem_Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new Uri("https://localhost:7049/"));

builder.Services.AddScoped(sp =>
{
    var handler =
        sp.GetRequiredService<JwtHandler>();

    handler.InnerHandler = new HttpClientHandler();

    var baseUri = sp.GetRequiredService<Uri>();

    return new HttpClient(handler)
    {
        BaseAddress = baseUri
    };
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtHandler>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();
