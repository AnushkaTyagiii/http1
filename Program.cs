using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using http1;
using Microsoft.ApplicationInsights.Channel;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Set up Application Insights
        services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
        services.AddSingleton(serviceProvider =>
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")
            };
            return telemetryConfiguration;
        });

        services.AddSingleton<TelemetryClient>();

        // Configure Entity Framework Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection")));
    })
    .Build();

host.Run();
