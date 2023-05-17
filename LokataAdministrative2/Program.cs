using Blazored.Modal;
using LokataAdministrative2;
using LokataAdministrative2.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredModal();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://www.lokatamanagement.com")});
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped<ICitationClient, CitationClient>();
builder.Services.AddScoped<IProvinceClient, ProvinceClient>();
builder.Services.AddScoped<ICityClient, CityClient>();
builder.Services.AddScoped<IBarangayClient, BarangayClient>();
builder.Services.AddScoped<IViolationCategoryClient, ViolationCatClient>();
builder.Services.AddScoped<IViolationClient, ViolationClient>();
builder.Services.AddScoped<IViolationFeeClient, ViolationFeeClient>();
builder.Services.AddScoped<IVehicleClient, VehicleClient>();
await builder.Build().RunAsync();
