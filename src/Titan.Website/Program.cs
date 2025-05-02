using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Titan.Website;
using Titan.Website.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registrar serviços
builder.Services.AddScoped<ProfissionalService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<DepoimentoService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<AgendamentoService>();

await builder.Build().RunAsync();
