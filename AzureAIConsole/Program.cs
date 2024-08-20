using AzureAIPromptFlowLibrary;
using AzureAIConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            // Load configuration from appsettings.json and other sources
            config.SetBasePath(AppContext.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args);
        })
        .ConfigureServices((hostContext, services) =>
        {
            // Retrieve the configuration
            IConfiguration configuration = hostContext.Configuration;

            // Register configuration to be available for dependency injection
            services.AddSingleton<IConfiguration>(configuration);

            // Register your services
            services.AddSingleton<IAzureAIPromptFlowService, AzureAIPromptFlowService>();
            services.AddSingleton<App>();
        });
}