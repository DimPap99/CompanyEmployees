﻿using Contracts.Interfaces;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
             services.AddCors(options =>
             {
                 options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
             });



        public static void ConfigureIISIntegration(this IServiceCollection services) =>
             services.Configure<IISOptions>(options => { });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();
        //configure the db context and send migration assmbly to the CompanyEmployees project
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
         services.AddDbContext<RepositoryContext>(opts => 
         opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("CompanyEmployees")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

    
    }
}