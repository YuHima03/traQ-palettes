using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Palettes.App.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddMemoryCache(builder.Configuration.GetSection("Cache:Memory").Bind);

            await builder.Build().RunAsync();
        }
    }
}
