using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Palettes.App.Configurations;
using Palettes.Infrastructure.Repository;

namespace Palettes.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configurations from .env files.
            LoadEnvFiles(builder.Configuration);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddAuthenticationStateSerialization();

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<TraqClientOptions>(builder.Configuration);

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(o => builder.Configuration.Bind("Session", o));

            // Authentication
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => builder.Configuration.Bind("Authentication:Cookie", o));
            builder.Services
                .AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            // Http Clients
            builder.Services.AddHttpClient();
            builder.Services.AddDefaultTraqHttpClient();
            builder.Services.AddAuthenticatedTraqHttpClient();

            // Database
            builder.Services.Configure<DbConnectionOptions>(builder.Configuration);
            builder.Services.AddRepositoryFactory((sp, options) =>
            {
                var dbConnectionString = sp.GetRequiredService<IOptions<DbConnectionOptions>>().Value.GetConnectionString();
                options.UseMySQL(dbConnectionString);
            });

            // Caching
            builder.Services.AddMemoryCache(builder.Configuration.GetSection("Cache:Memory").Bind);

            builder.Services.TryAddScoped<Api.IApiClientFactory, ApiHandlers.ApiHandlerFactory>();

            builder.Services.AddControllers();
            builder.Services.AddAntiforgery();

            builder.Services.AddSingleton(TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseAntiforgery();

            app.MapControllers();

            app.MapStaticAssets();
            app.MapRazorComponents<Components.App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }

        static void LoadEnvFiles(ConfigurationManager configurations)
        {
            var paths = configurations["env-files"].AsSpan();
            var ranges = paths.Split(';');
            foreach (var r in ranges)
            {
                var p = paths[r].ToString();
                if (File.Exists(p))
                {
                    configurations.AddIniStream(File.OpenRead(p));
                }
            }
        }
    }
}
