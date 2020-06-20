using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansOom.Grains;

namespace OrleansOom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .UseInMemoryReminderService()
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "dev";
                            opts.ServiceId = "OrleansOom";
                        })
                        .Configure<SiloMessagingOptions>(options =>
                        {
                            // reduced message timeout to ease promise break testing
                            options.ClientDropTimeout = TimeSpan.FromSeconds(2);
                            options.ResponseTimeout = TimeSpan.FromSeconds(3);
                            options.ResponseTimeoutWithDebugger = TimeSpan.FromSeconds(4);
                        })                        
                        .Configure<ClientMessagingOptions>(options =>
                        {
                            // reduced message timeout to ease promise break testing
                            options.ResponseTimeout = TimeSpan.FromSeconds(5);
                            options.ResponseTimeoutWithDebugger = TimeSpan.FromSeconds(6);
                        })
                        .Configure<EndpointOptions>(opts => { opts.AdvertisedIPAddress = IPAddress.Loopback; })
                        .AddStartupTask<GrainWithTimerStartup>();
                });
    }
}
