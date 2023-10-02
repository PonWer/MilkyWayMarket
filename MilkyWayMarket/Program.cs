using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MilkyWayMarket;
using MudBlazor.Services;
using RestSharp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
	var options = new RestClientOptions("https://api.github.com")
	{
		ThrowOnAnyError = true,
		MaxTimeout = 60_000
	};

	return new RestClient(options);
});

builder.Services.AddScoped<IHttpClientFactory,HttpClientFactory>(sp =>
{
	var httpClientFactory = new HttpClientFactory();

	httpClientFactory.Register("local", new HttpClient
	{
		BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
	});

	httpClientFactory.Register("github", new HttpClient
	{
		BaseAddress = new Uri("https://www.github.com/"),
		DefaultRequestHeaders =
		{
			{"Access-Control-Allow-Origin","*"}
		}
	});

	return httpClientFactory;
});



builder.Services.AddMudServices();

builder.Services.AddSingleton<IDataService, DataService>();

await builder.Build().RunAsync();
