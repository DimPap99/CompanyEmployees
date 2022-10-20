using CompanyEmployees.Extensions;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
        // Add services to the container.

        builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Support for CORS and IIS integration
    builder.Services.ConfigureCors();
    builder.Services.ConfigureIISIntegration();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.ConfigureLoggerService();


    //builder.Logging.ClearProviders();
    //builder.
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    else app.UseHsts();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseCors("CorsPolicy");


    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
    });

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();


    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
