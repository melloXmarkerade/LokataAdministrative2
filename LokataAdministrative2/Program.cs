using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2;
using LokataAdministrative2.Authentication;
using LokataAdministrative2.Services;
using LokataAdministrative2.Services.AdminClient;
using LokataAdministrative2.Services.ViolationsClient;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSweetAlert2();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://www.lokatamanagement.com") });
 
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());

builder.Services.AddScoped<ICitationClient, CitationClient>();
builder.Services.AddScoped<IProvinceClient, ProvinceClient>();
builder.Services.AddScoped<ICityClient, CityClient>();
builder.Services.AddScoped<IBarangayClient, BarangayClient>();
builder.Services.AddScoped<IViolationCategoryClient, ViolationCatClient>();
builder.Services.AddScoped<IViolationClient, ViolationClient>();
builder.Services.AddScoped<IViolationFeeClient, ViolationFeeClient>();
builder.Services.AddScoped<IVehicleClient, VehicleClient>();
builder.Services.AddScoped<IUserReqSubmissionClient, UserReqSubmissionClient>();
builder.Services.AddScoped<IUserRecSubmissionClient, UserRecSubmissionClient>();
builder.Services.AddScoped<IAdminClient, AdminClient>();
builder.Services.AddScoped<INotificationClient, NotificationClient>();
builder.Services.AddScoped<AdminAuthClient>();
builder.Services.AddScoped<UserClient>();
builder.Services.AddScoped<IStorageRateClient, StorageRateClient>();
builder.Services.AddScoped<ITowingRateClient, TowingRatesClient>();
builder.Services.AddScoped<IImpoundingAreaClient, ImpoundingAreaClient>();
builder.Services.AddScoped<IRequirementClient, RequirementClient>();
builder.Services.AddScoped<IPaymentChannelClient, PaymentChannelClient>();
await builder.Build().RunAsync();
