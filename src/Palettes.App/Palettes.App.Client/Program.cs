using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Palettes.Api;
using Palettes.App.Client.ApiClient;

namespace Palettes.App.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore()
                .AddAuthenticationStateDeserialization();

            builder.Services.AddMemoryCache(builder.Configuration.GetSection("Cache:Memory").Bind);

            builder.Services.AddSingleton(CreateDefaultHttpClient(builder));

            builder.Services.AddSingleton<IApiClientFactory, ApiClientFactory>();

            builder.Services.AddSingleton(TimeZoneInfo.CreateCustomTimeZone("JST", TimeSpan.FromHours(9), null, null));

            await builder.Build().RunAsync();
        }

        static HttpClient CreateDefaultHttpClient(WebAssemblyHostBuilder builder)
        {
            HttpClientHandler clientHandler = new()
            {
                AllowAutoRedirect = false,
            };
            HttpClient client = new(clientHandler)
            {
                BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress.TrimEnd('/')}/api/")
            };
            return client;
        }
    }
}
