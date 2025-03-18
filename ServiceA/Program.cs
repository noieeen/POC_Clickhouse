using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOpenTelemetry()
//     .ConfigureResource(opt => opt.AddService(serviceName: Assembly.GetCallingAssembly().GetName().Name!))
//     .WithTracing(opt => opt.AddAspNetCoreInstrumentation()
//         .AddHttpClientInstrumentation()
//         .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:4317"); }))
//     .WithMetrics(opt => opt.AddAspNetCoreInstrumentation()
//         .AddHttpClientInstrumentation()
//         .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:4317"); }));
//
//
// // Add OpenTelemetry Tracing
// // builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
// // {
// //     tracerProviderBuilder
// //         .AddAspNetCoreInstrumentation()
// //         .AddHttpClientInstrumentation()
// //         .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:4317"); });
// // });
//
// // Add OpenTelemetry Metrics
// // builder.Services.AddOpenTelemetryMetrics(meterProviderBuilder =>
// // {
// //     meterProviderBuilder
// //         .AddAspNetCoreInstrumentation()
// //         .AddHttpClientInstrumentation()
// //         .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:4317"); });
// // });
//
// // Add Logging (Vector -> ClickHouse)
// builder.Logging.AddOpenTelemetry(options =>
// {
//     options.SetResourceBuilder(OpenTelemetry.Resources.ResourceBuilder.CreateDefault().AddService(Assembly.GetCallingAssembly().GetName().Name!));
//     options.AddOtlpExporter(exporterOptions => { exporterOptions.Endpoint = new Uri("http://vector:8686"); });
// });

// 🔹 Configure OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:50051"); });
    })
    .WithMetrics(metricProviderBuilder =>
    {
        metricProviderBuilder
            .AddPrometheusExporter()
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter(options => { options.Endpoint = new Uri("http://otel-collector:50051"); });
    });

// 🔹 Configure Logging (Vector)
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;
    logging.AddOtlpExporter(options => { options.Endpoint = new Uri("http://vector:8686"); });
});

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddJsonConsole(options =>
    {
        options.IncludeScopes = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());
// .AddDbContextCheck<YourDbContext>("database");

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();