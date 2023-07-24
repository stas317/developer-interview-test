using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Services;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        try
        {
            services.GetRequiredService<Application>().Run(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public static IHostBuilder CreateHostBuilder(string[] strings)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IRebateService, RebateService>();
                services.AddSingleton<IProductDataStore, ProductDataStore>();
                services.AddSingleton<IRebateDataStore, RebateDataStore>();
                services.AddSingleton<Application>();
            });
    }
}
