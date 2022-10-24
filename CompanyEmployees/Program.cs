using CompanyEmployees.Extensions;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerManager();//NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

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

    //Set up NLog for DI
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //Content Negotiation
    builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCSVFormatter();
    
    //Use the LoggerService
    builder.Services.ConfigureLoggerService();

    //Configure the DbContext 
    builder.Services.ConfigureSqlContext(builder.Configuration);

    

    //Configure the RepositoryManager for DI
    builder.Services.ConfigureRepositoryManager();
    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    var app = builder.Build();
    app.ConfigureExceptionHandler(logger);
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseCors("CorsPolicy");
   
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
    });

    app.UseRouting();

    //Convention Based Routing
    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllerRoute(
    //        name: "default",
    //        pattern: "{controller=Home/{action=Index}/{id?}"
    //        );

    //});

    app.UseAuthorization();

    app.MapControllers();


    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.LogError( $"Stopped program because of exception: {exception}");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    logger.Shutdown();
}
