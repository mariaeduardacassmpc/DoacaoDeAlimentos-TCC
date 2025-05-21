using FrontDoacaoDeAlimentos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorInputMask;
using Microsoft.JSInterop;
using TCCDoacaoDeAlimentos;
using Blazored.Toast;
using Blazored.Modal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = 
    new Uri("https://localhost:7083/") });

var host = builder.Build();
await host.RunAsync();
